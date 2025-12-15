using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public class CellData
    {
        public bool Passable;
        public CellObject ContainedObject;
    }

    readonly int[] foodGranted = new int[]{5,6,7,10,11,12};
    
    public int Width;
    public int Height;
    public Tile[] GroundTiles;
    public Tile[] WallTiles;
    public PlayerController Player;
    public FoodObject FoodPrefab;
    public WallObject WallPrefab;
    public Sprite[] FoodSprite;
    public int MinFood = 2;
    [Tooltip("음식 최대 수(포함)")]
    public int MaxFood = 5;

    Tilemap tilemap;
    CellData[,] boardData;
    Grid grid;
    List<Vector2Int> emptyCellList;
    
    public void Init()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        grid = GetComponentInChildren<Grid>();
        emptyCellList = new List<Vector2Int>();
        boardData = new CellData[Width, Height];

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Tile tile;
                boardData[x, y] = new CellData();
                if (x == 0 || y == 0 || x == Width - 1 || y == Height - 1)
                {
                    tile = GetRandomTile(WallTiles);
                    boardData[x, y].Passable = false;
                }
                else
                {
                    tile = GetRandomTile(GroundTiles);
                    boardData[x, y].Passable = true;
                    emptyCellList.Add(new Vector2Int(x, y));
                }
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
        emptyCellList.Remove(new Vector2Int(1, 1));
        GenerateWall();
        GenerateFood();
    }

    Tile GetRandomTile(Tile[] tiles)
    {
        return tiles[Random.Range(0, tiles.Length)];
    }

    public Vector3 CellToWorld(Vector2Int cellIndex)
    {
        return grid.GetCellCenterWorld((Vector3Int)cellIndex);
    }

    public CellData GetCellData(Vector2Int cellIndex)
    {
        if (cellIndex.x < 0 || cellIndex.x >= Width
            || cellIndex.y < 0 || cellIndex.y >= Height)
        {
            return null;
        }    
        return boardData[cellIndex.x, cellIndex.y];
    }

    public void SetCellType(Vector2Int cellIndex, Tile tile)
    {
        tilemap.SetTile(new Vector3Int(
            cellIndex.x, cellIndex.y, 0), tile);    
    }

    public Tile GetCellTile(Vector2Int cellIndex)
    {
        return tilemap.GetTile<Tile>
            (new Vector3Int(cellIndex.x, cellIndex.y, 0));
    }
    
    void AddObject(CellObject obj, Vector2Int coord)
    {
        CellData data = boardData[coord.x, coord.y];
        obj.transform.position = CellToWorld(coord);
        data.ContainedObject = obj;
        obj.Init(coord);
    }

    void GenerateFood()
    {
        int foodCount = Random.Range(MinFood, MaxFood + 1);
        Debug.Log($"Food count : {foodCount}");
        for (int i = 0; i < foodCount; i++)
        {
            int randomIndex = Random.Range(0, emptyCellList.Count);
            Vector2Int coord = emptyCellList[randomIndex];

            emptyCellList.RemoveAt(randomIndex);
            CellData data = boardData[coord.x, coord.y];
            int foodType = Random.Range(0, FoodSprite.Length);
            FoodPrefab.GetComponent<SpriteRenderer>().sprite 
                = FoodSprite[foodType];
            FoodPrefab.SetGrantedValue(foodGranted[foodType]);
            FoodObject newFood = Instantiate(FoodPrefab);
            AddObject(newFood, coord);
        }
    }

    void GenerateWall()
    {
        int wallCount = Random.Range(6, 10);
        for (int i = 0; i < wallCount; i++)
        {
            int randomIndex = Random.Range(0, emptyCellList.Count);
            Vector2Int coord = emptyCellList[randomIndex];

            emptyCellList.RemoveAt(randomIndex);
            WallObject newWall = Instantiate(WallPrefab);
            AddObject(newWall, coord);
        }
    }
}
