using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sizes { Small, Medium, Large }
public enum Colors { Orange, Grey, Black }

public class Characteristics : MonoBehaviour
{
    public Colors color;
    public Sizes size;

    private GameManager gameManager;

    public void Start()
    {
        gameManager = GameManager.Instance;
        Setup();
    }

    private void Setup()
    {
        switch (color)
        {
            case Colors.Orange:
                GetComponentInChildren<SpriteRenderer>().color = gameManager.orange;
                break;
            case Colors.Grey:
                GetComponentInChildren<SpriteRenderer>().color = gameManager.grey;
                break;
            case Colors.Black:
                GetComponentInChildren<SpriteRenderer>().color = gameManager.black;
                break;
        }
    }
}
