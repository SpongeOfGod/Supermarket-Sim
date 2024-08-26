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
    public bool walking = true;

    public bool hasSearched;
    public bool isFirst;
    public bool firstSearchDone;

    [Header("Positions")]
    private Vector3 targetPosition;
    private Vector3 initialPosition;

    [Header("Components")]
    public ShelfList shelfList;

    private void Start()
    {
        shelfList = GameObject.FindGameObjectWithTag("ShelfManager").GetComponent<ShelfList>();
        initialPosition = transform.position;
        isFirst = false;
        firstSearchDone = false;
    }

    private void Update()
    {
        float currentSpeed = walking ? speed * speedMultiplier : speed;

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
            walking = false;
        }
    }

    public int QueuePosition => queuePosition;

    public bool FirstSearchDone { get => firstSearchDone; set => firstSearchDone = value; }

    public void SetQueuePosition(int positionIndex)
    {
        queuePosition = positionIndex;
    }

    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
    }
}
