using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject : CellObject
{
    [Tooltip("부서지는 벽 이미지 종류")]
    public int MaxWallType = 3;
    [Tooltip("MaxWallType의 두배로 설정")]
    public Tile[] ObstacleTile;
    public int MaxHealth = 3;
    int healthPoint;
    int wallType;
    Tile originalTile;

    public override void Init(Vector2Int inCell)
    {
        base.Init(inCell);
        healthPoint = MaxHealth;
        originalTile = GameManager.Instance.BoardManager.
            GetCellTile(cell);
        // MaxWallType중에 랜덤하게 하나를 고른다
        wallType = Random.Range(0, MaxWallType);
        GameManager.Instance.BoardManager.
            SetCellType(cell, ObstacleTile[wallType]);
    }
    public override bool PlayerWantsToEnter()
    {
        healthPoint--;
        if (healthPoint > 0)
        {
            if (healthPoint == 1)
            {
                // 바꿀 타일의 인덱스는 MaxWallType만큼 떨어져 있다
                int change = wallType + MaxWallType;
                GameManager.Instance.BoardManager.
                    SetCellType(cell, ObstacleTile[change]);
            }
            return false;
        }
        GameManager.Instance.BoardManager.
            SetCellType(cell, originalTile);
        Destroy(gameObject);
        return true;
    }
}
