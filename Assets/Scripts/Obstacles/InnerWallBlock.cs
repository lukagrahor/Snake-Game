using UnityEngine;

public class InnerWallBlock : MonoBehaviour, ISnakeHeadTriggerHandler, IBeeFrontTriggerHandler, IWaspFrontTriggerHandler, IDogTriggerHandler
{
    public void HandleEnemyFrontTrigger(Bee enemy)
    {
        enemy.Turn();
    }

    public void HandleTrigger(SnakeHead snakeHead)
    {
        snakeHead.Snake.HitWall();
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

    public void HandleTrigger(IState patrolState, Dog dog)
    {
        DogPatrolState currentState = (DogPatrolState)patrolState;
        currentState.ChangeLane();
    }
}
