using System.Collections.Generic;
using UnityEngine;

public class SnakeNormalState : IState
{
    SnakeHead snakeHead;
    SnakeHeadStateMachine stateMachine;
    LinkedList<float> rotationBuffer;
    public SnakeNormalState(SnakeHead snakeHead, SnakeHeadStateMachine stateMachine)
    {
        this.snakeHead = snakeHead;
    }

    public void Enter()
    {
        rotationBuffer = new LinkedList<float>();
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
