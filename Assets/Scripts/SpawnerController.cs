using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public Stack<GameObject> ObjectStack = new Stack<GameObject>();
    [SerializeField] private GameObject objectToAdd;
    [SerializeField] private GameObject[] gameObjects;
    [SerializeField] private float spaceBetween;
    [SerializeField] private Vector3 initialPos;

    private bool above;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        gameObjects = ObjectStack.ToArray();

        if (Input.GetButtonDown("Fire2") && ObjectStack.Count >= 0 && above) 
        {
            PopStack();
        }

        if (Input.GetButtonDown("Fire1") && ObjectStack.Count <= 4 && above)
        {
            GameObject a = Instantiate(objectToAdd, Vector3.zero, Quaternion.identity, transform);
            a.transform.position = initialPos + Vector3.back * spaceBetween * ObjectStack.Count + new Vector3(0,1f,0);
            Quaternion rotA = Quaternion.Euler(0, 0, 180);
            a.transform.rotation = rotA;
            ObjectStack.Push(a);
        }
    }

    public void PopStack() 
    {
        ObjectStack.TryPop(out GameObject plate);
        Destroy(plate);
    }

    private void OnMouseOver()
    {
        above = true;
    }

    private void OnMouseExit()
    {
        above = false;
    }

}
