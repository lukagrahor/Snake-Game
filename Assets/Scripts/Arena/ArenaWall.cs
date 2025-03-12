using UnityEngine;

public class ArenaWall : MonoBehaviour, ISnakeHeadTriggerHandler
{
    public void HandleTrigger(SnakeHead snakeHead)
    {
        snakeHead.GetHit();
    }
}
