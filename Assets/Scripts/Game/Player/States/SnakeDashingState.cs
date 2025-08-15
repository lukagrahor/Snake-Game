using UnityEngine;
using System;
using System.IO.Pipes;

public class SnakeDashingState : SnakePowerState
{
    float currentChargeTime;
    float maxChargeTime = 0.8f;
    float chargeTimeLimit = 3f;
    float currentCalculatedBiteRange;
    float moveSpeed = 2f;
    LineRenderer lineRenderer;

    //Quaternion biteMoveRotation;
    Vector3 biteMoveDirecton;

    public SnakeDashingState(SnakeHead snakeHead, LayerMask layersToHit, SnakeHeadStateMachine stateMachine) : base(snakeHead, layersToHit, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Dashing time!");
        snakeHead.Snake.MoveSpeed = moveSpeed;
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        snakeHead.transform.Translate(snakeHead.MoveSpeed * Time.deltaTime * Vector3.forward);
    }

    public override void SetRotation(float turnRotation)
    {

    }
}
