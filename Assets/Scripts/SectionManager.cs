using System.Collections.Generic;
using UnityEngine;

public class SectionManager : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject prefab;

    [Header("Position")]
    public int numberOfSections = 5;
    public Vector3 spacing = new Vector3(2f, 0f, 0f);
    public List<GameObject> sections = new List<GameObject>();

    [Header("State")]
    private Transform parent;
    private int lastInstanceCount = 0;

    private void Start()
    {
        parent = this.transform;
        UpdateInstances();
    }

    private void Update()
    {
        if (numberOfSections != lastInstanceCount)
        {
            UpdateInstances();
            lastInstanceCount = numberOfSections;
        }
    }

    public void UpdateInstances()
    {
        if (sections.Count > numberOfSections)
        {
            int excess = sections.Count - numberOfSections;
            for (int i = 0; i < excess; i++)
            {
                Destroy(sections[sections.Count - 1]);
                sections.RemoveAt(sections.Count - 1);
            }
        }
        else if (sections.Count < numberOfSections)
        {
            int toAdd = numberOfSections - sections.Count;
            for (int i = 0; i < toAdd; i++)
            {
                Vector3 position = GetPosition(sections.Count);
                GameObject newInstance = Instantiate(prefab, position, Quaternion.identity, parent);
                sections.Add(newInstance);
            }
        }

        for (int i = 0; i < sections.Count; i++)
        {
            sections[i].transform.localPosition = GetPosition(i);
        }
    }

    Vector3 GetPosition(int index)
    {
        if (index == 0)
        {
            return Vector3.zero;
        }

        int direction = (index % 2 == 0) ? 1 : -1;
        int step = (index + 1) / 2;

        float x = direction * step * spacing.x;
        return new Vector3(x, 0, 0);
    }

    public void AddInstance()
    {
        numberOfSections++;
        UpdateInstances();
    }

    public void RemoveInstance()
    {
        if (numberOfSections > 0)
        {
            numberOfSections--;
            UpdateInstances();
        }
    }
}
