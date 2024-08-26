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

        UpdateCustomer();
    }

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
        foreach (GameObject customer in queue)
        {
            if (customer == null) continue;
            CustomerBot bot = customer.GetComponent<CustomerBot>();
            if (bot != null && bot.QueuePosition == 1)
            {
                bot.ActivateSearch();
            }
        }
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
