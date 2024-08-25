using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerBot : MonoBehaviour
{
    [SerializeField] float speed = 10;
    [SerializeField] GameObject container;
    SpawnerController controller;
    void Start()
    {
        
    }

    public void RunUpdate()
    {
        Vector3.MoveTowards(this.transform.position, container.transform.position, speed * Time.deltaTime );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Muro"))
        {
            speed = -speed;
        }

        if (other.gameObject.CompareTag("Container"))
        {
            controller = other.gameObject.GetComponent<SpawnerController>(); ;

            Debug.Log("colisiono");
            grabFlakes();
        }
    }

    private void grabFlakes()
    {
        controller.PopStack();
        Debug.Log("se borra");
    }
}
