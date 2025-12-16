using UnityEngine;
using UnityEngine.Tilemaps;

public class ExitObject : CellObject
{
    public Tile EndTile;
    public override void Init(Vector2Int inCell)
    {
        base.Init(inCell);
        GameManager.Instance.BoardManager.
            SetCellTile(cell, EndTile);
    }
    public override void PlayerEntered()
    {
        GameManager.Instance.NewLevel();
    }
}
