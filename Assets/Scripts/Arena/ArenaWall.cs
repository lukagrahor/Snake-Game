using UnityEngine;

public class ArenaWall : MonoBehaviour, ISnakeHeadTriggerHandler, IBeeFrontTriggerHandler, IWaspFrontTriggerHandler
{
    public void HandleEnemyFrontTrigger(Bee enemy)
    {
        enemy.Turn();
    }

    public void HandleTrigger(SnakeHead snakeHead)
    {
        snakeHead.GetHit();
    }

    public void HandleTrigger(Wasp wasp)
    {
        wasp.Turn();
    }
    /*
    public void HandleTrigger(DogPatrolState patrolState, Dog dog)
    {
        patrolState.ChangeLane();
    }
    */
}
