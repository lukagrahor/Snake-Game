using UnityEngine;

public interface ISnakeInput
{
    void SubscribeToInput();
    void OnSnakeDeath();
    void OnSnakeRespawn();
}
