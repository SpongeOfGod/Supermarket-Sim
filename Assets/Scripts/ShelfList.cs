using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using static System.Collections.Specialized.BitVector32;

public class ShelfList : MonoBehaviour
{
    [SerializeField] private List<GameObject> shelves = new List<GameObject>();

    void Start()
    {
        FindAllShelves();
    }

    void FindAllShelves()
    {
        shelves.Clear();
        GameObject[] allShelves = GameObject.FindGameObjectsWithTag("Shelf");
        shelves.AddRange(allShelves);
    }

    public GameObject GetRandomShelf()
    {
        if (shelves.Count == 0)
        {
            Debug.LogWarning("No shelves available.");
            return null;
        }

        int randomIndex = Random.Range(0, shelves.Count);
        return shelves[randomIndex];
    }

    public GameObject GetRandomSection()
    {
        if (shelves.Count == 0)
        {
            Debug.LogWarning("No shelves available.");
            return null;
        }

        int randomIndex = Random.Range(0, shelves.Count);
        SectionManager sectionManager = shelves[randomIndex].GetComponent<SectionManager>();

        if (sectionManager != null && sectionManager.sections.Count > 0)
        {
            int rRandomIndex = Random.Range(0, sectionManager.sections.Count);
            GameObject section = sectionManager.sections[randomIndex];
            return section;
        }

        return null;
    }
}
