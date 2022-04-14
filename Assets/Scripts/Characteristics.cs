using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sizes { Small, Medium, Large }
public enum Colors { Orange, Grey, Black }

public class Characteristics : MonoBehaviour
{
    public Colors color;
    public Sizes size;

    public void Start()
    {
        Setup();
    }

    private void Setup()
    {
        switch (color)
        {
            case Colors.Orange:
                GetComponent<SpriteRenderer>().color = GameManager.Instance.orange;

                break;
            case Colors.Grey:
                GetComponent<SpriteRenderer>().color = GameManager.Instance.grey;

                break;
            case Colors.Black:
                GetComponent<SpriteRenderer>().color = GameManager.Instance.black;
                break;
        }
    }
}
