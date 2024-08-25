using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerBot : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    public bool isArranged;
    void Start()
    {
        
    }

    public void RunUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position , transform.position + Vector3.right,  speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Muro"))
        {
            speed = -speed;
        }
    }
}
