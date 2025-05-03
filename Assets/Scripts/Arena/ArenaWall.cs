using UnityEngine;

public class ArenaWall : MonoBehaviour, ISnakeHeadTriggerHandler, IBeeFrontTrigger
{
    public void HandleEnemyFrontTrigger(Bee enemy)
    {
        enemy.Turn();
    }

    public void HandleTrigger(SnakeHead snakeHead)
    {
        snakeHead.GetHit();
    }
    /*
    public void HandleTrigger(DogPatrolState patrolState, Dog dog)
    {
        patrolState.ChangeLane();
    }
    */
}
