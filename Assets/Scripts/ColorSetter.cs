using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Colors { Orange, Grey, Black }

public class ColorSetter : MonoBehaviour
{
    public Colors color;

    [Header("Colors")]
    public Color orange = Color.red;
    public Color grey = Color.grey;
    public Color black = Color.black;

    public void Start()
    {
        Setup();
    }

    private void Setup()
    {
        switch (color)
        {
            case Colors.Orange:
                GetComponentInChildren<SpriteRenderer>().color = orange;
                break;
            case Colors.Grey:
                GetComponentInChildren<SpriteRenderer>().color = grey;
                break;
            case Colors.Black:
                GetComponentInChildren<SpriteRenderer>().color = black;
                break;
        }
    }
}
