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
    [SerializeField, ReadOnly] private bool foundBox = false;
    [SerializeField, ReadOnly] private Transform targetBox; // This is set once the cat has found a box
    [SerializeField, ReadOnly] private bool inBox = false; // Stop moving when the cat is in the box

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
        if (!inBox)
        {
            Move();
            if (targetBox != null && Vector3.Distance(transform.position, targetBox.position) < boxDistanceTollarance)
            {
                inBox = true;
            }
        }
    }

    public void CreateRandomCat()
    {
        Array colors = Enum.GetValues(typeof(Colors));
        GetComponent<ColorSetter>().color = (Colors)colors.GetValue(UnityEngine.Random.Range(0, Enum.GetNames(typeof(Colors)).Length));

        Array sizes = Enum.GetValues(typeof(Sizes));
        size = (Sizes)sizes.GetValue(UnityEngine.Random.Range(0, Enum.GetNames(typeof(Sizes)).Length));
    }

    private void Move()
    {
        if (!foundBox)
        {
            CheckForEmptyBoxes();
        }
        
        transform.position += speed * Time.deltaTime * direction;
    }

    private void CheckForEmptyBoxes()
    {
        direction = Vector2.down; // If it doesn't find a hit on either side it will continue walking down

        // FIXME: this will be biased to the right. Add in every other time it goes left
        RaycastForBox(Vector2.left);
        RaycastForBox(Vector2.right);
    }

    private void RaycastForBox(Vector2 dir)
    {
        RaycastHit2D check = Physics2D.Raycast(transform.position, dir, 50f, walkToLayers);
        // BUG: The cat won't detect boxes behind other boxes
        if (check.collider != null && !check.collider.GetComponentInParent<BoxController>().IsOccupied && 
            EvaluateBoxSize(check.collider.GetComponentInParent<BoxController>()) && !foundBox)
        {
            direction = dir;
            foundBox = true;
            check.collider.GetComponentInParent<BoxController>().IsOccupied = true;
            targetBox = check.collider.transform;
        }
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
