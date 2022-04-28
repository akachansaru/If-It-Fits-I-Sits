using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Adapted from https://www.youtube.com/watch?v=gFpmJtO0NT4&ab_channel=TamaraMakesGames
public enum TileType { None, Empty, Filled, Overlap }

public class BoxGridPlacement : MonoBehaviour
{
    public GridLayout gridLayout;
    public Tilemap tilemapMain;
    public Tilemap tilemapTemp;

    public static BoxGridPlacement instance;

    private readonly Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

    private BoxController currBox; // This is the box that is selected to be moved
    private BoundsInt prevArea = new BoundsInt();

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        string tilePath = "Tiles/";
        tileBases.Add(TileType.None, null);
        tileBases.Add(TileType.Empty, Resources.Load<TileBase>(tilePath + "Empty"));
        tileBases.Add(TileType.Filled, Resources.Load<TileBase>(tilePath + "Filled"));
        tileBases.Add(TileType.Overlap, Resources.Load<TileBase>(tilePath + "Overlap"));
    }

    public void FollowBox()
    {
        ClearPrevArea();
        currBox.area.position = gridLayout.WorldToCell(currBox.transform.position);
        BoundsInt boxArea = currBox.area;

        if (IsInBounds(currBox))
        {
            TileBase[] baseArray = tilemapMain.GetTilesBlock(boxArea);
            TileBase[] tileArray = new TileBase[baseArray.Length];

            for (int i = 0; i < tileArray.Length; i++)
            {
                if (baseArray[i] == tileBases[TileType.Empty])
                {
                    // So far so good to place here
                    tileArray[i] = tileBases[TileType.Filled];
                }
                else
                {
                    // If one overlaps, fill the whole area with red and return
                    FillTiles(tileArray, TileType.Overlap);
                    break;
                }
            }

            tilemapTemp.SetTilesBlock(boxArea, tileArray);
        }
        prevArea = boxArea;
    }

    public void BoxSelected(BoxController box)
    {
        currBox = box;
    }

    public bool CanTakeArea(BoundsInt area)
    {
        TileBase[] baseArray = tilemapMain.GetTilesBlock(area);
        foreach (TileBase tile in baseArray)
        {
            if (tile != tileBases[TileType.Empty])
            {
                return false;
            }
        }
        return true;
    }

    public void TakeArea(BoundsInt area)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = tileBases[TileType.Filled];
        }

        tilemapMain.SetTilesBlock(area, array);
    }

    public void RemoveArea(BoundsInt area)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = tileBases[TileType.Empty];
        }

        tilemapMain.SetTilesBlock(area, array);
    }

    // I think this just snaps the box you're dragging
    public void PlaceOnGrid(Transform toPlace)
    {
        Vector3Int cellPos = tilemapMain.LocalToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        toPlace.localPosition = gridLayout.CellToLocalInterpolated(cellPos + new Vector3(0.5f, 0.5f, 0f));
    }

    private bool IsInBounds(BoxController box)
    {
        foreach(Renderer renderer in GameManager.Instance.gameBounds)
        {
            // FIXME: Need to make it so the offset works and doesn't show the red tile if it's partly over the path
            if (renderer.bounds.Contains(box.transform.position + box.GetOffset()))
            {
                return true;
            }
        }
        return false;
    }

    private void ClearPrevArea()
    {
        TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
        FillTiles(toClear, TileType.None);
        tilemapTemp.SetTilesBlock(prevArea, toClear);
    }

    private void FillTiles(TileBase[] tiles, TileType type)
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = tileBases[type];
        }
    }
}
