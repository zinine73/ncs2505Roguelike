using Unity.VisualScripting;
using UnityEngine;

public class Enemy : CellObject
{
    public int Health = 3;
    int currentHealth;
    void Awake()
    {
        GameManager.Instance.TurnManager.OnTick += TurnHappened;
    }
    void OnDestroy()
    {
        GameManager.Instance.TurnManager.OnTick -= TurnHappened;
    }
    public override void Init(Vector2Int inCell)
    {
        base.Init(inCell);
        currentHealth = Health;
    }
    public override bool PlayerWantsToEnter()
    {
        currentHealth--;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        return false;
    }
    bool MoveTo(Vector2Int coord)
    {
        var board = GameManager.Instance.BoardManager;
        var targetCell = board.GetCellData(coord);
        if (targetCell == null
            || !targetCell.Passable
            || targetCell.ContainedObject != null)
        {
            return false;
        }

        var currentCell = board.GetCellData(cell);
        currentCell.ContainedObject = null;
        targetCell.ContainedObject = this;
        cell = coord;
        transform.position = board.CellToWorld(coord);
        return true;
    }

    void TurnHappened()
    {
        var playerCell = GameManager.Instance.PlayerController.Cell;
        int xDist = playerCell.x - cell.x;
        int yDist = playerCell.y - cell.y;
        int absXDist = Mathf.Abs(xDist);
        int absYDist = Mathf.Abs(yDist);
        if ((xDist == 0 && absYDist == 1)
            || (yDist == 0 && absXDist == 1))
        {
            GameManager.Instance.ChangeFood(-3);
        }
        else
        {
            if (absXDist > absYDist)
            {
                if (!TryMoveInX(xDist))
                {
                    TryMoveInY(yDist);
                }
            }
            else
            {
                if (!TryMoveInY(yDist))
                {
                    TryMoveInX(xDist);
                }
            }
        }
    }

    bool TryMoveInX(int xDist)
    {
        if (xDist > 0)
        {
            return MoveTo(cell + Vector2Int.right);
        }
        return MoveTo(cell + Vector2Int.left);
    }
    bool TryMoveInY(int yDist)
    {
        if (yDist > 0)
        {
            return MoveTo(cell + Vector2Int.up);
        }
        return MoveTo(cell + Vector2Int.down);
    }
}
