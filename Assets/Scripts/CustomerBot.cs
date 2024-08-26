using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerBot : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private CustomerMovement customerMovement;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float speedMultiplier = 3f;

    [Header("State")]
    private int queuePosition;
    private bool walking = true;

    private bool hasSearched;
    private bool isFirst;
    private bool firstSearchDone;
    public bool done;


    [Header("Positions")]
    private Vector3 targetPosition;
    private Vector3 initialPosition;

    [Header("Components")]
    public ShelfList shelfList;


    //Getters and Setters
    public int QueuePosition => queuePosition;
    public bool FirstSearchDone { get => firstSearchDone; set => firstSearchDone = value; }
    public Vector3 TargetPosition { get => targetPosition; set => targetPosition = value; }
    public float Speed { get => speed; set => speed = value; }
    public bool Walking { get => walking; set => walking = value; }


    private void Start()
    {
        initialPosition = transform.position;
        customerMovement = gameObject.GetComponent<CustomerMovement>();
        customerMovement.Initialize(this, gameObject);
        shelfList = GameObject.FindGameObjectWithTag("ShelfManager").GetComponent<ShelfList>();
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
                StartCoroutine(customerMovement.MoveToPosition(initialPosition, currentSpeed));
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
            customerMovement.RotateTowardsDirection(targetPosition);
        }
    }

    public void ActivateSearch()
    {
        if (queuePosition == 1 && !hasSearched)
        {
            StartCoroutine(customerMovement.SearchForShelf());
            hasSearched = true;
            walking = false;
        }
    }

    public void SetQueuePosition(int positionIndex)
    {
        queuePosition = positionIndex;
    }

    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
    }
}
