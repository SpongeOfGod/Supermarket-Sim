using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Temble : MonoBehaviour
{
    [SerializeField] private float despairTime;
    [SerializeField] private Vector3 offset = new Vector3();
    [SerializeField] private GameObject moneyText;

    private int currentScore;
    private float elapsedTime;
    private bool despair;
    private Vector3 initialPosition;

    void Start()
    {
        currentScore = GameManager.score;
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float randomX = Random.Range(0f, offset.x);
        float randomY = Random.Range(0f, offset.y);


        if (currentScore == GameManager.score) 
        {
            elapsedTime += Time.deltaTime;

            if(elapsedTime > despairTime) 
            {
                despair = true;
            }
        }
        else 
        {
            currentScore = GameManager.score;
            elapsedTime = 0;
            despair = false;
            transform.position = initialPosition;

            if (moneyText != null)
            {
                moneyText.SetActive(false);
            }
        }

        if (despair) 
        {
            transform.position = initialPosition + new Vector3(randomX, randomY, 0f);
            if(moneyText != null) 
            {
                moneyText.SetActive(true);
            }
        }
    }
}
