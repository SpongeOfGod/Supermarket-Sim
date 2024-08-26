using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorMesh : MonoBehaviour
{
    void Start()
    {
        Color randomColor = SetColor();
        ApplyToChildren(transform, randomColor);
    }

    void ApplyToChildren(Transform parent, Color color)
    {
        foreach (Transform child in parent)
        {
            MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.material.color = color;
            }
        }
    }

    Color SetColor()
    {
        int colorType = Random.Range(0, 10);

        if (colorType == 0)
        {
            return Color.black;
        }
        else if (colorType == 1)
        {
            return Color.white;
        }
        else if (colorType >= 2 && colorType <= 4)
        {
            return Color.red;
        }
        else if (colorType == 5)
        {
            return Color.yellow;
        }
        else
        {
            float hue = Random.Range(0f, 1f);
            float saturation = Random.Range(0.7f, 1f);
            float brightness = Random.Range(0.8f, 1f);
            return Color.HSVToRGB(hue, saturation, brightness);
        }
    }
}
