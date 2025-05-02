using UnityEngine;

public class ArenaWall : MonoBehaviour, ISnakeHeadTriggerHandler, ITestEnemyFrontTrigger
{
    public void HandleEnemyFrontTrigger(TestEnemy enemy)
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
