using CustomEditorScripts;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoxController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField, ReadOnly] private bool isClicked = false;

    public Sizes size;
    public BoundsInt area; // This is the area the box should take up in the grid
    public GameObject sprite;
    public GameObject hitBox;
    public int boxLayer;

    public bool IsPlaced { get; private set; } // If it's placed on the grid

    public bool IsOccupied { get; set; } // If a cat is sitting in it

    public void Start()
    {
        switch (size)
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

    public void CreateRandomBox()
    {
        Array colors = Enum.GetValues(typeof(Colors));
        GetComponent<ColorSetter>().color = (Colors)colors.GetValue(UnityEngine.Random.Range(0, Enum.GetNames(typeof(Colors)).Length));

        Array sizes = Enum.GetValues(typeof(Sizes));
        size = (Sizes)sizes.GetValue(UnityEngine.Random.Range(0, Enum.GetNames(typeof(Sizes)).Length));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!IsPlaced && !isClicked)
        {
            transform.parent = null;
            hitBox.layer = 0; // To make it so the cats don't detect the box while draggin
            isClicked = true;
            BoxGridPlacement.instance.BoxSelected(this);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (CanBePlaced())
        {
            isClicked = false;
            hitBox.layer = boxLayer; // Once placed, the cats should detect the box
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
