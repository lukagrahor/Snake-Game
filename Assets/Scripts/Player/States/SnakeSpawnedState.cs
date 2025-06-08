using System.Collections.Generic;
using UnityEngine;

public class SnakeSpawnedState : ISnakeState, IRegularMovement
{
    SnakeHead snakeHead;
    SnakeHeadStateMachine stateMachine;
    float startingMoveSpeed = 0.5f;
    float transitionDuration = 2f;
    CountDown timer;
    public SnakeSpawnedState(SnakeHead snakeHead, SnakeHeadStateMachine stateMachine)
    {
        this.snakeHead = snakeHead;
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        snakeHead.Snake.MoveSpeed = startingMoveSpeed;
        snakeHead.SetToTransparent();
        timer = new CountDown(transitionDuration);
        timer.TimeRanOut += TransitionToNormalState;
        timer.Start();
    }

    public void Exit()
    {
        snakeHead.SetToSolid();
    }

    public void Update()
    {
        timer.Update();
        snakeHead.Move();
    }

    public void OnGridBlockStay(Collider other)
    {
        // ignore the y axis
        Vector3 snakeHeadPosition = new(snakeHead.transform.position.x, 0f, snakeHead.transform.position.z);
        Vector3 gridBlockPosition = new(other.transform.position.x, 0f, other.transform.position.z);
        Vector3 nextGridBlockPosition = new(snakeHead.NextBlock.transform.position.x, 0f, snakeHead.NextBlock.transform.position.z);

        Vector3 movementDirection = RotationToMovementVector(snakeHead.GetRotation());
        Vector3 directionToBlock = nextGridBlockPosition - snakeHeadPosition;
        float dotProduct = Vector3.Dot(movementDirection, directionToBlock.normalized);
        // dot product nam pove ali vektorja kažeta v isto ali nasprotno smer
        // && distance <= 0.1f, brez tega se je kocka obrnila ko je bila že na robu kocke, kar je izgledalo èudno
        float distance = Vector3.Distance(snakeHeadPosition, gridBlockPosition);
        if (distance <= 0.03f || (dotProduct < 0 && distance <= 0.1f))
        {
            if (snakeHead.RotationBuffer.Count > 0)
            {
                // snap to the place of the grid block
                snakeHead.transform.position = new Vector3(gridBlockPosition.x, snakeHead.transform.position.y, gridBlockPosition.z);

                Rotate();
                snakeHead.LastRotationBlock = other.GetComponent<GridObject>();
            }
        }
    }

    public void Rotate()
    {
        if (snakeHead.RotationBuffer.Count <= 0)
        {
            return;
        }

        snakeHead.transform.Rotate(0, snakeHead.RotationBuffer.First.Value, 0);
        snakeHead.Snake.SetTorsoRotation(snakeHead.RotationBuffer.First.Value);
        snakeHead.RotationBuffer.RemoveFirst();
    }

    public void SetRotation(float turnRotation)
    {
        if (snakeHead.RotationBuffer.Count == 2)
        {
            return;
        }
        snakeHead.RotationBuffer.AddLast(turnRotation);
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

    void TransitionToNormalState()
    {
        stateMachine.TransitionTo(stateMachine.NormalState);
    }
}
