using System.Collections;
using UnityEngine;

public class Enemy : CellObject
{
    readonly int hashMove = Animator.StringToHash("MOVING");
    readonly int hashAttack = Animator.StringToHash("ATTACK");
    readonly int hashDamage = Animator.StringToHash("DAMAGE");
    public int Health = 3;
    public int Reward = 3;
    int currentHealth;
    Vector3 moveTarget;
    Animator anim;
    SpriteRenderer sr;
    bool isMoving;
    float moveSpeed = 3.0f;

    void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
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
        anim.SetTrigger(hashDamage);
        currentHealth--;
        if (currentHealth <= 0)
        {
            // add food to player
            GameManager.Instance.ChangeFood(Reward);
            Destroy(gameObject);
        }
        return false;
    }
    bool MoveTo(Vector2Int coord, bool immediate = false)
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
        if (immediate)
        {
            isMoving = false;
            transform.position = board.CellToWorld(cell);
        }
        else
        {
            isMoving = true;
            moveTarget = board.CellToWorld(cell);            
        }
        anim.SetBool(hashMove, isMoving);
        return true;
    }

    IEnumerator AttackToPlayer()
    {
        if (currentHealth <= 0) yield return null;
        GameManager.Instance.PlayerController.Lock = true;
        yield return new WaitForSeconds(1f);
        anim.SetTrigger(hashAttack);   
        GameManager.Instance.ChangeFood(-3);
        GameManager.Instance.PlayerController.GetDamage();
        yield return new WaitForSeconds(1f);
        GameManager.Instance.PlayerController.Lock = false;
    }

    void TurnHappened()
    {
        var playerCell = GameManager.Instance.PlayerController.Cell;
        int xDist = playerCell.x - cell.x;
        int yDist = playerCell.y - cell.y;
        sr.flipX = xDist > 0;    
        int absXDist = Mathf.Abs(xDist);
        int absYDist = Mathf.Abs(yDist);
        if ((xDist == 0 && absYDist == 1) || (yDist == 0 && absXDist == 1))
        {
            StartCoroutine(AttackToPlayer());
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

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, moveTarget, moveSpeed * Time.deltaTime);
            if (transform.position == moveTarget)
            {
                isMoving = false;
                anim.SetBool(hashMove, false);
            }
        }
    }
}
