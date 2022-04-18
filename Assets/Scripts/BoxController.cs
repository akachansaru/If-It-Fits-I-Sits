using CustomEditorScripts;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoxController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField, ReadOnly] private bool isClicked = false;

    public BoundsInt area; // This is the area the box should take up in the grid
    public bool IsPlaced { get; private set; } // If it's placed on the grid

    public bool IsOccupied { get; set; } // If a cat is sitting in it

    public void Start()
    {
        switch (GetComponent<Characteristics>().size)
        {
            case Sizes.Small:
                area = new BoundsInt(Vector3Int.zero, new Vector3Int(1, 1, 1));
                break;
            case Sizes.Medium:
                area = new BoundsInt(Vector3Int.zero, new Vector3Int(2, 2, 1));
                break;
            case Sizes.Large:
                area = new BoundsInt(Vector3Int.zero, new Vector3Int(3, 3, 1));
                break;
        }
    }

    public void Update()
    {
        if (isClicked)
        {
            DragBox();
            BoxGridPlacement.instance.FollowBox();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!IsPlaced && !isClicked)
        {
            isClicked = true;
            BoxGridPlacement.instance.BoxSelected(this);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (CanBePlaced())
        {
            isClicked = false;
            BoxGridPlacement.instance.BoxSelected(null);
            BoxGridPlacement.instance.PlaceOnGrid(transform);
            Place();
        }
    }

    public bool CanBePlaced()
    {
        Vector3Int positionInt = BoxGridPlacement.instance.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        if (BoxGridPlacement.instance.CanTakeArea(areaTemp))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Place()
    {
        Vector3Int positionInt = BoxGridPlacement.instance.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        IsPlaced = true;
        BoxGridPlacement.instance.TakeArea(areaTemp);
    }

    private void DragBox()
    {
        transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
    }
}
