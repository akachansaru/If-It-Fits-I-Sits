using System;
using UnityEngine;
using UnityEngine.EventSystems;
using CustomEditorScripts;

public class BoxController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField, ReadOnly] private bool isClicked = false;
    [SerializeField, ReadOnly] private bool isOccupied = false;

    public Sizes size;
    public BoundsInt area; // This is the area the box should take up in the grid
    [SerializeField] private GameObject sprite;
    public GameObject hitBox;
    public int boxLayer;

    [Space, Header("Offset Settings")]
    [SerializeField] private float offset;
    [SerializeField] private float scale;

    public bool IsPlaced { get; private set; } // If it's placed on the grid

    public bool IsOccupied { get { return isOccupied; } set { isOccupied = value; } } // If a cat is sitting in it

    private bool isOverTrash = false;

    public void Start()
    {
        // Set everything up so the sprite looks like it's in the right place and the hit box is centered
        sprite.transform.localPosition = Vector2.one * offset;
        sprite.transform.localScale = Vector2.one * scale;
        hitBox.transform.localPosition = Vector2.one * offset;
        GetComponent<BoxCollider2D>().offset = Vector2.one * offset;
        GetComponent<BoxCollider2D>().size = Vector2.one * scale;

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

    public Vector3 GetOffset()
    {
        return sprite.transform.localPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!IsPlaced && !isClicked)
        {
            transform.parent = null; // Remove it from the box selection place once picked up
            hitBox.layer = 0; // To make it so the cats don't detect the box while dragging
            isClicked = true;
            BoxGridPlacement.instance.BoxSelected(this);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isOverTrash)
        {
            Destroy(gameObject);
        }
        else
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

    public void Remove()
    {
        Vector3Int positionInt = BoxGridPlacement.instance.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        IsPlaced = false;
        BoxGridPlacement.instance.RemoveArea(areaTemp);
    }

    private void DragBox()
    {
        transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Trash"))
        {
            isOverTrash = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Trash"))
        {
            isOverTrash = false;
        }
    }
}
