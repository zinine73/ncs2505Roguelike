using UnityEngine;
using System;
public class TurnManager
{
    public event Action OnTick;

    int turnCount;

    public TurnManager()
    {
        turnCount = 1;
    }

    public void Tick()
    {
        turnCount++;
        // if (OnTick != null)
        // {
        //     OnTick.Invoke();
        // }
        OnTick?.Invoke();
    }
}
