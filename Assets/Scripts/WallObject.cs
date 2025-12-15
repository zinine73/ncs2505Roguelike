using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject : CellObject
{
    public Tile ObstacleTile;
    public int MaxHealth = 3;
    int healthPoint;
    Tile originalTile;

    public override void Init(Vector2Int inCell)
    {
        base.Init(inCell);
        healthPoint = MaxHealth;
        originalTile = GameManager.Instance.BoardManager.
            GetCellTile(cell);
        GameManager.Instance.BoardManager.
            SetCellType(cell, ObstacleTile);
    }
    public override bool PlayerWantsToEnter()
    {
        healthPoint--;
        if (healthPoint > 0)
        {
            return false;
        }
        GameManager.Instance.BoardManager.
            SetCellType(cell, originalTile);
        Destroy(gameObject);
        return true;
    }
}
