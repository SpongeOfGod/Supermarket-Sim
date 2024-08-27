using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRectTransform : MonoBehaviour
{
    public float rotationSpeed = 100f;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        rectTransform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
