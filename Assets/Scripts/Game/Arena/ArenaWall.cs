using UnityEngine;

public class ArenaWall : MonoBehaviour, ISnakeHeadTriggerHandler, IBeeFrontTriggerHandler, IWaspFrontTriggerHandler
{
    public void HandleEnemyFrontTrigger(Bee enemy)
    {
        enemy.Turn();
    }

    public void HandleTrigger(SnakeHead snakeHead)
    {
        snakeHead.HitWall();
    }

    public void HandleTrigger(Wasp wasp)
    {
        wasp.Turn();
        WaspStateMachine stateMachine = wasp.Ai.waspStateMachine;

        if (stateMachine == null || stateMachine.CurrentState == null) return;

        if (stateMachine.CurrentState == stateMachine.ChargeState)
        {
            stateMachine.ChargeState.CoolDown();
        }
    }
}
