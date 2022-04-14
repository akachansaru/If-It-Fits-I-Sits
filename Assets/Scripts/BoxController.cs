using CustomEditorScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoxController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    [SerializeField, ReadOnly] private bool isClicked = false;

    private GameManager gameManager;

    public void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isClicked = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isClicked = false;
        SnapToGrid();
    }

    public void Update()
    {
        if (isClicked)
        {
            DragBox();
        }
    }

    private void OnDrawGizmos()
    {
        //SnapToGrid();
    }

    private void SnapToGrid()
    {
        Vector3 grid = gameManager.grid;

        // Make sure grid sizes are > 0
        if (grid.x == 0)
        {
            grid = new Vector3(1, grid.y, grid.z);
        }
        if (grid.y == 0)
        {
            grid = new Vector3(grid.x, 1, grid.z);
        }
        if (grid.z == 0)
        {
            grid = new Vector3(grid.x, grid.y, 1);
        }

        // Bring the box back into the bounds of the game if the player dragged it too far
        if (transform.position.x < gameManager.left)
        {
            transform.position = new Vector3(gameManager.left, transform.position.y, transform.position.z);
        }
        if (transform.position.x > gameManager.right)
        {
            transform.position = new Vector3(gameManager.right, transform.position.y, transform.position.z);
        }
        if (transform.position.y < gameManager.bottom)
        {
            transform.position = new Vector3(transform.position.x, gameManager.bottom, transform.position.z);
        }
        if (transform.position.y > gameManager.top)
        {
            transform.position = new Vector3(transform.position.x, gameManager.top, transform.position.z);
        }

        // Snap to the grid
        transform.position = new Vector3(Mathf.Round(transform.position.x / grid.x) * grid.x,
                                         Mathf.Round(transform.position.y / grid.y) * grid.y,
                                         Mathf.Round(transform.position.z / grid.z) * grid.z);
    }

    private void DragBox()
    {
        transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
    }
}
