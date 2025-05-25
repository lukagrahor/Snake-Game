using UnityEngine;

public abstract class Enemy : MonoBehaviour, IBiteTriggerHandler, ISnakeHeadTriggerHandler, ISnakeTorsoTriggerHandler
{
    protected abstract void GetHit();
    public abstract void Setup(int col, int row, int gridSize);

    public void HandleBiteTrigger(SnakeHead snakeHead)
    {
        GetHit();
    }

    public void HandleTrigger(SnakeHead snakeHead)
    {
        snakeHead.GetHit();
    }

    public void HandleTorsoTrigger(SnakeTorso torso)
    {
        torso.GetHit();
    }
}
