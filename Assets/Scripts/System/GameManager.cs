using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameManager : MonoBehaviour
{
    [Header("Colors")]
    public Color orange = Color.red;
    public Color grey = Color.grey;
    public Color black = Color.black;

    public static GameManager Instance { get; private set; }
    public static bool IsPaused { get; private set; }

    private void Awake()
    {
        Instance = this;
    }


    public void GameOver()
    {
        Pause();
        Debug.Log("Game Over");
    }

    public static void Pause()
    {
        Time.timeScale = 0;
        IsPaused = true;
    }

    public static void Unpause()
    {
        Time.timeScale = 1;
        IsPaused = false;
    }
}
