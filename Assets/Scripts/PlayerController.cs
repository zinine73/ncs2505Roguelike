using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    readonly int hashMove = Animator.StringToHash("MOVING");
    readonly int hashAttack = Animator.StringToHash("ATTACK");
    readonly int hashDamage = Animator.StringToHash("DAMAGE"); 
    BoardManager board;
    Vector2Int cellPosition;
    Vector3 moveTarget;
    Animator anim;
    SpriteRenderer sr; // 왼쪽 이동/공격 모션을 위해 추가
    bool isGameOver;
    bool isMoving;
    float moveSpeed = 3.0f;

    public bool Lock { get; set; }
    public Vector2Int Cell => cellPosition;
    public void GetDamage() => anim.SetTrigger(hashDamage);    

    void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void Init()
    {
        isGameOver = false;
        anim.enabled = true;
        Lock = false;
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
        anim.SetBool(hashMove, isMoving);
    }

    public void GameOver()
    {
        isGameOver = true;
        // 게임오버시 플레이어 행동 멈추기
        anim.enabled = false;
    }

    void Update()
    {
        if (Lock) return;
        
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
            sr.flipX = false; // 오른쪽 보기
        }
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x--;
            hasMoved = true;
            sr.flipX = true; // 왼쪽 보기
        }

        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, moveTarget, moveSpeed * Time.deltaTime);
            if (transform.position == moveTarget)
            {
                isMoving = false;
                anim.SetBool(hashMove, false);
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
                if (cellData.ContainedObject == null)
                {
                    MoveTo(newCellTarget, false);
                }
                else
                {
                    if (cellData.ContainedObject.PlayerWantsToEnter())
                    {
                        MoveTo(newCellTarget, false);
                        cellData.ContainedObject.PlayerEntered();
                    }
                    else
                    {
                        anim.SetTrigger(hashAttack);
                    }
                }
                GameManager.Instance.TurnManager.Tick();
            }
        }
    }
}
