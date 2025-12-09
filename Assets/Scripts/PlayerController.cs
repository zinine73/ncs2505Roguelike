using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    BoardManager board;
    Vector2Int cellPosition;

    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        board = boardManager;
        MoveTo(cell);
    }

    public void MoveTo(Vector2Int cell)
    {
        cellPosition = cell;
        transform.position = board.CellToWorld(cellPosition);
    }

    void Update()
    {
        Vector2Int newCellTarget = cellPosition;
        bool hasMoved = false;

        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y++;
            hasMoved = true;
        }
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y--;
            hasMoved = true;
        }
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x++;
            hasMoved = true;
        }
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x--;
            hasMoved = true;
        }

        if (hasMoved)
        {
            BoardManager.CellData cellData 
                = board.GetCellData(newCellTarget);
            if (cellData != null && cellData.Passable)
            {
                MoveTo(newCellTarget);
            }
        }
    }
}
