using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorMesh : MonoBehaviour
{
    MeshRenderer MeshRenderer;
    [SerializeField] Color Color;
    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer = GetComponent<MeshRenderer>();
        MeshRenderer.material.color = Color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
