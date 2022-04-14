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

    [Space]
    public Vector3 grid;
    public float left;
    public float right;
    public float bottom;
    public float top;

    public float width;
    public float height;
    public float numColumns;
    public float numRows;
    public static GameManager Instance { get; private set; }
    public static bool IsPaused { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DrawGrid();
    }

    private void DrawGrid()
    {
        width = right - left;
        height = top - bottom;

        numColumns = width / grid.x;
        numRows = height / grid.y;
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
