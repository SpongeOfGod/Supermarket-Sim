using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;

public class ScoreKiller : MonoBehaviour
{
    private void Update()
    {
        StartCoroutine(updateScore());
    }

    private IEnumerator updateScore()
    {
        yield return new WaitForSeconds(1.01f);
        Destroy(gameObject);
    }
}
