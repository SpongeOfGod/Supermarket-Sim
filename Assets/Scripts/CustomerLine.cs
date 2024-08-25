using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerLine : MonoBehaviour
{
    [SerializeField] private GameObject customer; 
    [SerializeField] private float timeToSpawn;
    [SerializeField] private float timeToReachSections;
    [SerializeField] Vector3 waitingLine;
    [SerializeField] float offsetZ;
    [SerializeField] private List<SpawnerController> spawnerControllers = new List<SpawnerController>();
    private Queue<GameObject> queue = new Queue<GameObject>();
    private float currentTime;
    private bool onSearchMode;
    private bool hasFinished;
    private bool onPosition;
    [SerializeField] private Vector3 locationToGo;


    void Start()
    {

    }


    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= timeToSpawn && queue.Count <= 3) 
        {
            CreateCustomer();
        }

        if(!onSearchMode && queue.Count == 4) 
        {
            onSearchMode = true;
        }

        if (onSearchMode && !onPosition) 
        {
            StartCoroutine(AlignWithSections());
        }

        if (onPosition) 
        {
            GameObject a = queue.Peek();
            a.TryGetComponent<CustomerBot>(out CustomerBot component);
            component.RunUpdate();
        }
    }

    public void CreateCustomer() 
    {
        GameObject a = Instantiate(customer, Vector3.zero, Quaternion.identity, transform);
        a.transform.position = waitingLine + Vector3.back * offsetZ * queue.Count + new Vector3(0, 1f, 0);
        Quaternion rotA = Quaternion.Euler(0, 180, 0);
        a.transform.rotation = rotA;
        queue.Enqueue(a);
        currentTime = 0;
    }

    IEnumerator AlignWithSections() 
    {
        float cTime = 0;
        GameObject a = queue.Peek();
        Vector3 initialPos = a.transform.position;
        while (cTime < timeToReachSections && !onPosition) 
        {
            cTime += Time.deltaTime;
            a.transform.position = Vector3.Lerp(initialPos, locationToGo, cTime / timeToReachSections);
            yield return null;
        }
        a.transform.position = locationToGo;
        onPosition = true;
        StopCoroutine(AlignWithSections());
        yield return null;
    }
}
