using UnityEngine;

public class SnakeBitingState : IState
{
    SnakeHead snakeHead;
    SnakeHeadStateMachine stateMachine;
    public SnakeBitingState(SnakeHead snakeHead, SnakeHeadStateMachine stateMachine)
    {
        this.snakeHead = snakeHead;
    }

    public void Enter()
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        throw new System.NotImplementedException();
    }
}
