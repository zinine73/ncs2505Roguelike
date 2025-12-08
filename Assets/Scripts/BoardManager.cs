using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public class CellData
    {
        public bool Passable;
    }

    public int Width;
    public int Height;
    public Tile[] GroundTiles;
    public Tile[] WallTiles;
    Tilemap tilemap;
    CellData[,] boardData;

    void Start()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        boardData = new CellData[Width, Height];

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Tile tile;
                boardData[x, y] = new CellData();
                if (x == 0 || y == 0 || x == Width - 1 || y == Height - 1)
                {
                    //tile = WallTiles[Random.Range(0, WallTiles.Length)];
                    tile = GetRandomTile(WallTiles);
                    boardData[x, y].Passable = false;
                }
                else
                {
                    //tile = GroundTiles[Random.Range(0, GroundTiles.Length)];
                    tile = GetRandomTile(GroundTiles);
                    boardData[x, y].Passable = true;
                }
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }

    Tile GetRandomTile(Tile[] tiles)
    {
        return tiles[Random.Range(0, tiles.Length)];
    }

    void Update()
    {
        
    }
}
