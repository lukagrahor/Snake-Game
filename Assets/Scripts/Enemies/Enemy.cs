using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IBiteTriggerHandler, ISnakeHeadTriggerHandler, ISnakeTorsoTriggerHandler
{
    public Action enemyDied;
    protected abstract void GetHit();
    public abstract void Setup(int col, int row, int gridSize);
    public abstract void SetupAI(Snake snake, ArenaGrid grid);

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
