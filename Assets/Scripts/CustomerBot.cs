using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerBot : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float waitTime = 0.25f;
    [SerializeField] private float speedMultiplier = 3f;
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private Vector3 targetOffset = Vector3.zero;
    [SerializeField] private Transform parentForFlakes;

    [Header("State")]
    public int queuePosition;
    public bool done;

    private bool hasSearched;
    private bool isFirst;
    private bool firstSearchDone;

    [Header("Positions")]
    private Vector3 targetPosition;
    private Vector3 initialPosition;

    [Header("Components")]
    private ShelfList shelfList;

    private void Start()
    {
        shelfList = GameObject.FindGameObjectWithTag("ShelfManager").GetComponent<ShelfList>();
        initialPosition = transform.position;
        isFirst = false;
        firstSearchDone = false;
    }

    private void Update()
    {
        float currentSpeed = hasSearched ? speed : speed * speedMultiplier;

        if (!isFirst)
        {
            if (Vector3.Distance(transform.position, initialPosition) > 0.1f)
            {
                StartCoroutine(MoveToPosition(initialPosition, currentSpeed));
            }
            else
            {
                isFirst = true;
            }
        }
        else if (targetPosition != Vector3.zero && Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);
            transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.z);
            RotateTowardsDirection(targetPosition);
        }
    }

    public void ActivateSearch()
    {
        if (queuePosition == 1 && !hasSearched)
        {
            StartCoroutine(SearchForShelf());
            hasSearched = true;
        }
    }

    public int QueuePosition => queuePosition;

    public void SetQueuePosition(int positionIndex)
    {
        queuePosition = positionIndex;
    }

    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
    }

    private IEnumerator MoveToPosition(Vector3 destination, float moveSpeed)
    {
        while (Vector3.Distance(transform.position, destination) > 0.1f)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
            transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.z);
            RotateTowardsDirection(destination);
            yield return null;
        }
        transform.position = new Vector3(destination.x, transform.position.y, destination.z);
    }

    private IEnumerator SearchForShelf()
    {
        if (!firstSearchDone)
        {
            yield return new WaitForSeconds(0.5f);
            firstSearchDone = true;
        }

        GameObject shelf = shelfList.GetRandomShelf();
        if (shelf != null)
        {
            SectionManager sectionManager = shelf.GetComponent<SectionManager>();
            if (sectionManager != null && sectionManager.sections.Count > 0)
            {
                int randomIndex = Random.Range(0, sectionManager.sections.Count);
                GameObject section = sectionManager.sections[randomIndex];
                if (section != null)
                {
                    Vector3 sectionPosition = section.transform.position;
                    SetTargetPosition(sectionPosition + targetOffset);
                    yield return MoveToPosition(targetPosition, speed);

                    yield return LookAtSection();

                    SpawnerController spawner = section.GetComponent<SpawnerController>();
                    if (spawner != null && spawner.ObjectStack.Count > 0)
                    {
                        Debug.Log("Misión Cumplida :)");
                        GameObject cereal = spawner.PopStack();
                        if(cereal != null) 
                        {
                            Animator animator = cereal.GetComponent<Animator>();
                            if(animator != null) 
                            {
                                animator.SetBool("despawn", false);
                            }
                            done = true;
                            cereal.transform.parent = parentForFlakes;
                            cereal.transform.position = parentForFlakes.transform.position + new Vector3(0, 1, 1);
                        }
                    }
                    else
                    {
                        Debug.Log("Intentando una sección aleatoria :(");
                        yield return new WaitForSeconds(waitTime);
                        StartCoroutine(NextSection());
                    }
                }
            }
        }
    }

    private IEnumerator LookAtSection()
    {
        while (Mathf.Abs(transform.rotation.eulerAngles.y) > 0.1f)
        {
            Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);
    }

    private IEnumerator NextSection()
    {
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(SearchForShelf());
    }

    private void RotateTowardsDirection(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
        }
    }
}
