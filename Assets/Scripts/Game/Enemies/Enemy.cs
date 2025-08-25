using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IBiteTriggerHandler, ISnakeHeadTriggerHandler, ISnakeTorsoTriggerHandler, ISpitTriggerHandler, IWaspFrontTriggerHandler, IBeeFrontTriggerHandler, IArenaKillBoxTriggerHandler
{
    public Action enemyDied;
    protected abstract void GetHit();
    public abstract void Setup(int col, int row, int gridSize);
    public abstract void SetupAI(Snake snake, ArenaGrid grid);

    public void HandleBiteTrigger(SnakeHead snakeHead)
    {
        GetHit();
    }

    public void HandleSpitTrigger()
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

    public void HandleTrigger(Wasp wasp)
    {
        wasp.Turn();
        WaspStateMachine stateMachine = wasp.Ai.waspStateMachine;
        if (stateMachine.CurrentState == stateMachine.ChargeState)
        {
            stateMachine.ChargeState.CoolDown();
            GetHit();
        }
    }

    public void HandleEnemyFrontTrigger(Bee bee)
    {
        bee.Turn();
    }

    public void HandleKillBoxTrigger()
    {
        GetHit();
    }
}
