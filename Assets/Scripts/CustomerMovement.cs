using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class CustomerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Vector3 targetOffset = Vector3.zero;
    [SerializeField] private Transform parentForFlakes;
    [SerializeField] private float waitTime = 0.25f;
    [SerializeField] private float rotationSpeed = 15f;

    [Header("Prefabs")]
    [SerializeField] private GameObject successPrefab;
    [SerializeField] private GameObject failurePrefab;

    CustomerBot customerBot;
    GameObject CustomerObject;
    
    public void Initialize(CustomerBot customer, GameObject customerGameObject) 
    {
        customerBot = customer;
        CustomerObject = customerGameObject;
    }

    public IEnumerator MoveToPosition(Vector3 destination, float moveSpeed)
    {
        while (Vector3.Distance(CustomerObject.transform.position, destination) > 0.1f)
        {
            Vector3 newPosition = Vector3.MoveTowards(CustomerObject.transform.position, destination, moveSpeed * Time.deltaTime);
            CustomerObject.transform.position = new Vector3(newPosition.x, CustomerObject.transform.position.y, newPosition.z);
            RotateTowardsDirection(destination);
            yield return null;
        }
        CustomerObject.transform.position = new Vector3(destination.x, CustomerObject.transform.position.y, destination.z);
    }

    public IEnumerator SearchForShelf()
    {
        if (!customerBot.FirstSearchDone)
        {
            yield return new WaitForSeconds(0.5f);
            customerBot.FirstSearchDone = true;
        }

        GameObject shelf = customerBot.shelfList.GetRandomSection();
        if (shelf != null)
        {
            Vector3 sectionPosition = shelf.transform.position;
            customerBot.SetTargetPosition(sectionPosition + targetOffset);
            yield return MoveToPosition(customerBot.TargetPosition, customerBot.Speed);

            yield return LookAtSection();

            SpawnerController spawner = shelf.GetComponent<SpawnerController>();
            if (spawner != null && spawner.ObjectStack.Count > 0)
            {
                customerBot.Walking = true;
                Debug.Log("Misión Cumplida :)");
                GameObject cereal = spawner.PopStack();
                if (cereal != null)
                {
                    Animator animator = cereal.GetComponent<Animator>();
                    if (animator != null)
                    {
                        animator.SetBool("despawn", false);
                    }
                    cereal.transform.parent = parentForFlakes;
                    cereal.transform.position = parentForFlakes.transform.position + new Vector3(0, 1, 1);
                    if (successPrefab != null)
                    {
                        GameObject successObject = Instantiate(successPrefab, parentForFlakes);
                        successObject.transform.localPosition = new Vector3(0, 3.75f, 0);
                    }
                    yield return new WaitForSeconds(waitTime);
                    customerBot.done = true;
                }
            }
            else
            {
                Debug.Log("Intentando una sección aleatoria :(");
                if (failurePrefab != null)
                {
                    GameObject failureObject = Instantiate(failurePrefab, parentForFlakes);
                    failureObject.transform.localPosition = new Vector3(0, 3.75f, 0);
                }
                yield return new WaitForSeconds(waitTime);
                StartCoroutine(NextSection());
            }
        }
    }

    public IEnumerator LookAtSection()
    {
        while (Mathf.Abs(CustomerObject.transform.rotation.eulerAngles.y) > 0.1f)
        {
            Quaternion targetRotation = Quaternion.Euler(CustomerObject.transform.rotation.eulerAngles.x, 0, CustomerObject.transform.rotation.eulerAngles.z);
            CustomerObject.transform.rotation = Quaternion.Slerp(CustomerObject.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
        CustomerObject.transform.rotation = Quaternion.Euler(CustomerObject.transform.rotation.eulerAngles.x, 0, CustomerObject.transform.rotation.eulerAngles.z);
    }

    public IEnumerator NextSection()
    {
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(SearchForShelf());
    }

    public void RotateTowardsDirection(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, customerBot.Speed * Time.deltaTime);
        }
    }
}
