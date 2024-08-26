using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static int score;


    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
        }

        if(instance != this) 
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }



    public static void AddScore() 
    {
        score++;
    }
}
