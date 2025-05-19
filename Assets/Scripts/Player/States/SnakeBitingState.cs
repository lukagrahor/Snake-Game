using UnityEngine;
using System;

public class SnakeBitingState : ISnakeState
{
    SnakeHead snakeHead;
    SnakeHeadStateMachine stateMachine;

    //Quaternion biteMoveRotation;
    Vector3 biteMoveDirecton;
    public SnakeBitingState(SnakeHead snakeHead, SnakeHeadStateMachine stateMachine)
    {
        this.snakeHead = snakeHead;
    }

    public void Enter()
    {
        SetBiteMovementDirection();
    }

    public void Exit()
    {
        snakeHead.transform.forward = biteMoveDirecton;
    }

    public void Update()
    {
        MoveWhileBiting();
    }

    public void SetBiteMovementDirection()
    {
        //biteMoveRotation = Quaternion.Euler(0f, snakeHead.GetRotation(), 0f);
        biteMoveDirecton = RotationToMovementVector(snakeHead.GetRotation());
    }

    public void MoveWhileBiting()
    {
        // ne sme se takoj prekinit, ker èe ne gre kaèa off the grid --> mogoèe buljše vrnt igralca nazaj na prejšnjo rotacijo
        if (biteMoveDirecton == null) return;
        snakeHead.transform.Translate(snakeHead.MoveSpeed * Time.deltaTime * biteMoveDirecton, Space.World);
    }

    public void SetRotation(float turnRotation)
    {
        float biteMoveRotation = MovementVectorToRotation(biteMoveDirecton);
        float wrongDirection = -1f;
        float currentRotation = snakeHead.GetRotation();
        Debug.Log("biteMoveRotation " + biteMoveRotation);
        Debug.Log("currentRotation " + currentRotation);
        float nextRotation = currentRotation + turnRotation;
        if (biteMoveRotation - 180 >= 0) wrongDirection = biteMoveRotation - 180f;
        else if (biteMoveRotation + 180 <= 360f) wrongDirection = biteMoveRotation + 180f;
        // don't allow the snake head to rotate into its tail
        if(nextRotation != wrongDirection) snakeHead.transform.Rotate(0, turnRotation, 0);
    }

    Vector3 RotationToMovementVector(float rotation)
    {
        // rotacije niso zmeraj tako kot bi si želel
        // 90.000001 --> pri rotaciji pride do float precision errors, zato zaokoržim
        rotation = Mathf.Round(rotation);
        return rotation switch
        {
            0 => new Vector3(0f, 0f, 1f),
            90 => new Vector3(1f, 0f, 0f),
            180 => new Vector3(0f, 0f, -1f),
            270 => new Vector3(-1f, 0f, 0f),
            _ => new Vector3(0f, 0f, 0f),
        };
    }

    float MovementVectorToRotation(Vector3 movementVector)
    {
        movementVector = movementVector.normalized;
        if (movementVector == new Vector3(0f, 0f, 1f)) return 0f;
        else if (movementVector == new Vector3(1f, 0f, 0f)) return 90f;
        else if (movementVector == new Vector3(0f, 0f, -1f)) return 180f;
        else if (movementVector == new Vector3(-1f, 0f, 0f)) return 270f;
        return -1f;
    }
}
