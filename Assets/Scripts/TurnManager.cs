using UnityEngine;

public class TurnManager
{
    int turnCount;

    public TurnManager()
    {
        turnCount = 1;
    }

    public void Tick()
    {
        turnCount++;
        Debug.Log($"Current : {turnCount}");
    }
}
