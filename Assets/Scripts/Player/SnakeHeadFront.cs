using UnityEngine;

public class SnakeHeadFront : MonoBehaviour, IEnemyFrontTrigger
{
    [SerializeField] SnakeHead snakeHead;

    public void HandleEnemyFrontTrigger(StationaryEnemy enemy)
    {
        snakeHead.GetSnake().GetHit();
    }

    private void OnTriggerEnter(Collider other)
    {
        var enteredObject = other.GetComponent<IFrontTriggerHandler>();
        enteredObject?.HandleFrontTrigger();
    }
}
