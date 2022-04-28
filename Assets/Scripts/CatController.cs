using CustomEditorScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour
{
    public Sizes size;
    public float speed = 1;
    public LayerMask walkToLayers;
    public float boxDistanceTollarance = 0.05f;

    [Header("Sizes")]
    public float small = 0.5f;
    public float medium = 0.75f;
    public float large = 1f;

    private Vector3 direction;
    [SerializeField, ReadOnly] private bool inCenter = false; // This will be false until the cat makes it to the middle and can walk down

    [SerializeField, ReadOnly] private bool foundBox = false;
    [SerializeField, ReadOnly] private Transform targetBox; // This is set once the cat has found a box
    [SerializeField, ReadOnly] public bool inBox = false; // Stop moving when the cat is in the box

    public void Start()
    {
        switch (size)
        {
            case Sizes.Small:
                transform.localScale = Vector3.one * small;
                break;
            case Sizes.Medium:
                transform.localScale = Vector3.one * medium;
                break;
            case Sizes.Large:
                transform.localScale = Vector3.one * large;
                break;
        }
    }

    public void Update()
    {
        if (!GameManager.IsPaused)
        {
            if (!inBox)
            {
                if (!inCenter && CheckIfCentered())
                {
                    direction = Vector2.down;
                }

                if (!foundBox && inCenter)
                {
                    CheckForEmptyBoxes();
                }

                Move();

                // BUG: get a better way of detecting when it's in a box. Sometimes the cat moves past it
                if (targetBox != null && Vector3.Distance(transform.position, targetBox.position) < boxDistanceTollarance)
                {
                    inBox = true;
                }
            }
        }
    }

    private bool CheckIfCentered()
    {
        if (direction.x < 0 && transform.position.x < 0)
        {
            inCenter = true;
            return true;
        }

        if (direction.x > 0 && transform.position.x > 0)
        {
            inCenter = true;
            return true;
        }

        return false;
    }

    public void CreateRandomCat(Vector3 initialDirection)
    {
        direction = initialDirection;

        Array colors = Enum.GetValues(typeof(Colors));
        GetComponent<ColorSetter>().color = (Colors)colors.GetValue(UnityEngine.Random.Range(0, Enum.GetNames(typeof(Colors)).Length));

        Array sizes = Enum.GetValues(typeof(Sizes));
        size = (Sizes)sizes.GetValue(UnityEngine.Random.Range(0, Enum.GetNames(typeof(Sizes)).Length));
    }

    public void CreateCat(Sizes size, Vector3 initialDirection, float speed)
    {
        this.size = size;
        direction = initialDirection;
        this.speed = speed;

        Array colors = Enum.GetValues(typeof(Colors));
        GetComponent<ColorSetter>().color = (Colors)colors.GetValue(UnityEngine.Random.Range(0, Enum.GetNames(typeof(Colors)).Length));
    }

    private void Move()
    {     
        transform.position += speed * Time.deltaTime * direction;
    }

    private void CheckForEmptyBoxes()
    {
        direction = Vector2.down; // If it doesn't find a hit on either side it will continue walking down

        // FIXME: this will be biased to the left. Add in every other time it goes right
        if (!RaycastForBox(Vector2.left))
        {
            RaycastForBox(Vector2.right);
        }
    }

    private bool RaycastForBox(Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 50f, walkToLayers);

        // Cat will go to the first free box in the row
        if (hit && !hit.collider.GetComponentInParent<BoxController>().IsOccupied &&
            EvaluateBoxSize(hit.collider.GetComponentInParent<BoxController>()))
        {
            direction = dir;
            foundBox = true;
            hit.collider.GetComponentInParent<BoxController>().IsOccupied = true;
            targetBox = hit.collider.transform;
            return true;
        }
        return false;

        //RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dir, 50f, walkToLayers);

        //if (hits.Length > 0 && !foundBox)
        //{
        //    // Cat will go to the first free box in the row
        //    for (int i = 0; i < hits.Length; i++)
        //    {
        //        if (!hits[i].collider.GetComponentInParent<BoxController>().IsOccupied &&
        //            EvaluateBoxSize(hits[i].collider.GetComponentInParent<BoxController>()))
        //        {
        //            direction = dir;
        //            foundBox = true;
        //            hits[i].collider.GetComponentInParent<BoxController>().IsOccupied = true;
        //            targetBox = hits[i].collider.transform;
        //            break;
        //        }
        //    }
        //}
    }

    /// <summary>
    /// Check if a cat will fit in a box. Cats can fit in boxes their size or larger
    /// </summary>
    /// <param name="box"></param>
    /// <returns></returns>
    private bool EvaluateBoxSize(BoxController box)
    {
        Sizes boxSize = box.size;
        switch (size)
        {
            case Sizes.Small:
                return true; // Small cats fit in all boxes
            case Sizes.Medium:
                return boxSize == Sizes.Medium || boxSize == Sizes.Large;
            case Sizes.Large:
                return boxSize == Sizes.Large; // Large cats only fit in large boxes
            default:
                Debug.LogWarning("Size not found");
                return false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GameOver"))
        {
            GameManager.Instance.GameOver();
        }
    }
}
