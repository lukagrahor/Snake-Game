using UnityEngine;

public class InnerWallBlock : MonoBehaviour, ISnakeHeadTriggerHandler
{
    public void HandleTrigger(SnakeHead snakeHead)
    {
        snakeHead.Snake.HitWall();
    }
}
