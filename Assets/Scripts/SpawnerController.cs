using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public Stack<GameObject> ObjectStack = new Stack<GameObject>();

    [Header("Prefab")]
    public GameObject objects;

    [Header("Position")]
    [SerializeField] private Vector3 initialPos;
    [SerializeField] private Vector3 offset;

    [Header("Material")]
    [SerializeField] private Material highlightMaterial;
    private Material originalMaterial;
    private MeshRenderer meshRenderer;

    [Header("State")]
    public bool mouseAbove;

    private void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        if (meshRenderer != null)
        {
            originalMaterial = meshRenderer.material;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire2") && ObjectStack.Count > 0 && mouseAbove)
        {
            PopStack();
        }

        if (Input.GetButtonDown("Fire1") && ObjectStack.Count <= 4 && mouseAbove)
        {
            GameObject a = Instantiate(objects, Vector3.zero, Quaternion.identity, transform);
            a.transform.localPosition = initialPos + Vector3.Scale(offset, new Vector3(0, 0, ObjectStack.Count));
            a.transform.rotation = Quaternion.Euler(0, 0, 180);
            ObjectStack.Push(a);
        }
    }

    public GameObject PopStack()
    {
        if (ObjectStack.TryPop(out GameObject plate))
        {
            Animator animator = plate.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("despawn", true);
                StartCoroutine(WaitAndReturn(plate, 0.1f));
                return plate;
            }
            else
            {
                Destroy(plate);
                return null;
            }
        }
        return null;
    }

    private IEnumerator WaitAndReturn(GameObject obj, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }

    private void OnMouseOver()
    {
        mouseAbove = true;
        if (meshRenderer != null)
        {
            meshRenderer.material = highlightMaterial;
        }
    }

    private void OnMouseExit()
    {
        mouseAbove = false;
        if (meshRenderer != null)
        {
            meshRenderer.material = originalMaterial;
        }
    }
}
