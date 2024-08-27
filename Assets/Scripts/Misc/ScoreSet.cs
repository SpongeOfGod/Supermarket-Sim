using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ScoreSet : MonoBehaviour
{
    private TextMeshProUGUI m_TextMeshProUGUI;
    [SerializeField] private Animator t_Animator;
    [SerializeField] private Animator c_Animator;
    [SerializeField] private GameObject scorePlus;
    private float actualScore;

    private void Start()
    {
        m_TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    private void Update() 
    {
        if(actualScore != GameManager.score) 
        {
            actualScore = GameManager.score;
            t_Animator.Play("Score_UP");
            c_Animator.Play("CoinIN");
            StartCoroutine(updateScore());
        }
    }

    private IEnumerator updateScore()
    {
        yield return new WaitForSeconds(0.48f);

        m_TextMeshProUGUI.text = "Cash: $" + actualScore;
        Instantiate(scorePlus, transform);
    }
}
