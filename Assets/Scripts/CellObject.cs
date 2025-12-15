using UnityEngine;

public class CellObject : MonoBehaviour
{
    protected Vector2Int cell;
    public virtual void Init(Vector2Int inCell)
    {
        cell = inCell;
    }
    public virtual void PlayerEntered()
    {
        
    }
    public virtual bool PlayerWantsToEnter()
    {
        return true;
    }
}
