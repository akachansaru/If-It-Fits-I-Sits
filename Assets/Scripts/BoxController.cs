using CustomEditorScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoxController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    [SerializeField, ReadOnly] private bool isClicked = false;

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
        SnapToGrid();
    }

    private void SnapToGrid()
    {
        Vector3 grid = GameManager.Instance.grid;

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

        transform.position = new Vector3(Mathf.Round(transform.position.x / grid.x) * grid.x,
                                         Mathf.Round(transform.position.y / grid.y) * grid.y,
                                         Mathf.Round(transform.position.z / grid.z) * grid.z);
    }

    private void DragBox()
    {
        transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
    }
}
