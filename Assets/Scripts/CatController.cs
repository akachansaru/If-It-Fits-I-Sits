using CustomEditorScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour
{
    public float speed = 1;
    public LayerMask walkToLayers;
    public float boxDistanceTollarance = 0.05f;

    private Vector3 direction;
    [SerializeField, ReadOnly] private bool foundBox = false;
    [SerializeField, ReadOnly] private Transform targetBox; // This is set once the cat has found a box
    [SerializeField, ReadOnly] private bool inBox = false; // Stop moving when the cat is in the box

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
        // TODO: Cats can either only go in boxes of their size OR they can go in boxes their size or bigger
        RaycastHit2D check = Physics2D.Raycast(transform.position, dir, 50f, walkToLayers);
        if (check.collider != null && !check.collider.GetComponentInParent<BoxController>().IsOccupied && 
            EvaluateBoxSize(check.collider.GetComponentInParent<BoxController>()) && !foundBox)
        {
            direction = dir;
            foundBox = true;
            check.collider.GetComponentInParent<BoxController>().IsOccupied = true;
            targetBox = check.collider.transform;
        }
    }

    private bool EvaluateBoxSize(BoxController box)
    {
        Sizes boxSize = box.GetComponentInParent<Characteristics>().size;
        switch (GetComponent<Characteristics>().size)
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
