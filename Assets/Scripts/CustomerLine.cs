using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerLine : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject customerPrefab;

    [Header("Position")]
    [SerializeField] private int maxCustomers = 4;
    [SerializeField] private float distanceBetweenCustomers = 1.5f;
    [SerializeField] private Vector3 initialPosition;

    [Header("State")]
    private Queue<GameObject> queue = new Queue<GameObject>();
    private bool onSearchMode;

    void Start()
    {
        UpdatePositions();
    }

    void Update()
    {
        if (queue.Count < maxCustomers)
        {
            CreateCustomer();
        }

        if (!onSearchMode && queue.Count >= maxCustomers)
        {
            if (IsAtInitialPosition())
            {
                onSearchMode = true;
                StartCoroutine(ActivateSearchMode());
            }
        }

<<<<<<< Updated upstream
        if (onSearchMode && !onPosition) 
        {
            IEnumerator coroutine = AlignWithSections();
            StartCoroutine(coroutine);
            RearrangeCustomers();
        }
=======
        UpdateCustomer();
    }
>>>>>>> Stashed changes

    public void CreateCustomer()
    {
        GameObject newCustomer = Instantiate(customerPrefab, transform.position, Quaternion.identity, transform);
        CustomerBot bot = newCustomer.GetComponent<CustomerBot>();
        if (bot != null)
        {
            bot.SetQueuePosition(queue.Count);
        }
        queue.Enqueue(newCustomer);
        UpdatePositions();
    }

    private void UpdatePositions()
    {
        int position = 0;
        foreach (GameObject customer in queue)
        {
            if (customer == null) continue;
            Vector3 targetPosition = initialPosition + new Vector3(0, 0, position * distanceBetweenCustomers);
            CustomerBot bot = customer.GetComponent<CustomerBot>();
            if (bot != null)
            {
                bot.SetTargetPosition(targetPosition);
                bot.SetQueuePosition(position + 1);
            }
            position++;
        }
    }

    private IEnumerator ActivateSearchMode()
    {
<<<<<<< Updated upstream
        GameObject a = Instantiate(customer, Vector3.zero, Quaternion.identity, transform);
        a.transform.position = waitingLine + Vector3.back * offsetZ * queue.Count + new Vector3(0, 1f, 0);
        Quaternion rotA = Quaternion.Euler(0, 180, 0);
        a.transform.rotation = rotA;
        queue.Enqueue(a);
        currentTime = 0;
    }

    public void RearrangeCustomers() 
    {
        foreach(GameObject gameObject in queue) 
        {
            if(gameObject != queue.Peek() && !gameObject.GetComponent<CustomerBot>().isArranged) 
            {
                StartCoroutine(MoveCustomersInLine(gameObject));
            }
        }
    }

    IEnumerator AlignWithSections() 
    {
        float cTime = 0;
        GameObject a = queue.Peek();
        Vector3 initialPos = a.transform.position;
        while (cTime < timeToReachSections && !onPosition) 
=======
        foreach (GameObject customer in queue)
>>>>>>> Stashed changes
        {
            if (customer == null) continue;
            CustomerBot bot = customer.GetComponent<CustomerBot>();
            if (bot != null && bot.QueuePosition == 1)
            {
                bot.ActivateSearch();
            }
        }
<<<<<<< Updated upstream
        a.transform.position = locationToGo;
        onPosition = true;
        StopCoroutine("AlignWithSections");
        yield return null;
    }

    IEnumerator MoveCustomersInLine(GameObject customer) 
    {
        float cTime = 0;
        Vector3 initialPos = customer.transform.position;
        while(cTime < timeToReachSections && onSearchMode && !onPosition) 
        {
            cTime += Time.deltaTime;
            customer.transform.position = Vector3.Lerp(initialPos, initialPos + new Vector3(0, 0, 2), cTime / timeToReachSections);
            yield return null;
        }
        customer.transform.position = initialPos + new Vector3(0, 0, 2);
        customer.GetComponent<CustomerBot>().isArranged = true;
        StopCoroutine("MoveCustomersInLine");
=======
>>>>>>> Stashed changes
        yield return null;
        onSearchMode = false;
    }

    private bool IsAtInitialPosition()
    {
        if (queue.Count > 0)
        {
            GameObject firstCustomer = queue.Peek();
            if (firstCustomer == null) return false;

            CustomerBot firstCustomerBot = firstCustomer.GetComponent<CustomerBot>();
            if (firstCustomerBot != null)
            {
                Vector3 currentPosition = firstCustomer.transform.position;
                Vector3 targetPosition = initialPosition;
                return Vector3.Distance(currentPosition, targetPosition) < 0.1f;
            }
        }
        return false;
    }


    public void DequeueFirst()
    {
        if (queue.Count > 0)
        {
            GameObject firstCustomer = queue.Dequeue();
            Destroy(firstCustomer);
            UpdatePositions();
        }
    }

    private void UpdateCustomer()
    {
        if (queue.Count > 0)
        {
            GameObject firstCustomer = queue.Peek();
            if (firstCustomer != null)
            {
                CustomerBot bot = firstCustomer.GetComponent<CustomerBot>();
                if (bot != null && bot.done)
                {
                    DequeueFirst();
                }
            }
        }
    }
}
