using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    BoardManager board;
    Vector2Int cellPosition;
    Vector3 moveTarget;
    Animator anim;
    bool isGameOver;
    bool isMoving;
    float moveSpeed = 3.0f;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Init()
    {
        isGameOver = false;
    }

    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        board = boardManager;
        MoveTo(cell);
    }

    public void MoveTo(Vector2Int cell, bool immediate = true)
    {
        cellPosition = cell;
        if (immediate)
        {
            isMoving = false;
            transform.position = board.CellToWorld(cellPosition);
        }
        else
        {
            isMoving = true;
            moveTarget = board.CellToWorld(cellPosition);
        }
        anim.SetBool("MOVING", isMoving);
    }

    public void GameOver()
    {
        isGameOver = true;
    }

    void Update()
    {
        if (isGameOver)
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                GameManager.Instance.StartNewGame();
            }
            return;
        }

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

        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, moveTarget, moveSpeed * Time.deltaTime);
            if (transform.position == moveTarget)
            {
                isMoving = false;
                anim.SetBool("MOVING", false);
                var cellData = board.GetCellData(cellPosition);
                if (cellData.ContainedObject != null)
                {
                    cellData.ContainedObject.PlayerEntered();
                }
            }
            return;    
        }

        if (hasMoved)
        {
            BoardManager.CellData cellData 
                = board.GetCellData(newCellTarget);
            if (cellData != null && cellData.Passable)
            {
                GameManager.Instance.TurnManager.Tick();
                if (cellData.ContainedObject == null)
                {
                    MoveTo(newCellTarget, false);
                }
                else if (cellData.ContainedObject.PlayerWantsToEnter())
                {
                    MoveTo(newCellTarget, false);
                    cellData.ContainedObject.PlayerEntered();
                }
            }
        }
    }
}
