using UnityEngine;
using System;
using System.IO.Pipes;

public abstract class SnakePowerState : ISnakeState
{
    protected SnakeHead snakeHead;
    protected LayerMask layersToHit;
    protected SnakeHeadStateMachine stateMachine;
    public SnakePowerState(SnakeHead snakeHead, LayerMask layersToHit, SnakeHeadStateMachine stateMachine)
    {
        this.snakeHead = snakeHead;
        this.layersToHit = layersToHit;
        this.stateMachine = stateMachine;
    }

    public abstract void Enter();

    public abstract void Exit();

    public abstract void SetRotation(float turnRotation);

    public abstract void Update();
}
