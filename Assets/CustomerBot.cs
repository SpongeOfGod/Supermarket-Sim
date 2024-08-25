using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerBot : MonoBehaviour
{
    [SerializeField] float speed = 10;
    void Start()
    {
        
    }

    public void RunUpdate()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Muro"))
        {
            speed = -speed;
        }
    }
}
