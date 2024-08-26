using UnityEngine;
using System.Collections.Generic;

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
}
