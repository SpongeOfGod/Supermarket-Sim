using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;

public class ScoreKiller : MonoBehaviour
{
    [SerializeField] private float killTime = 1.01f;

    private void Update()
    {
        StartCoroutine(updateScore());
    }

    private IEnumerator updateScore()
    {
        yield return new WaitForSeconds(killTime);
        Destroy(gameObject);
    }
}
