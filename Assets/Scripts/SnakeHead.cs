using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SnakeHead : MonoBehaviour, ISnakePart
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //public event Action onRotate;
    //float moveRotationY = 0f;
    float moveSpeed = 0f;
    bool lastSnakePart = true;
    private bool hasSnapped = false;

    Snake snake;
    GridObject nextBlock;
    LinkedList<float> rotationBuffer;

    void Awake()
    {
        rotationBuffer = new LinkedList<float>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    public void Setup(float moveSpeed, float moveRotation, Snake snake, Vector3 scale)
    {
        this.snake = snake;
        transform.SetParent(snake.transform);
        transform.localScale = scale;

        SetMoveSpeed(moveSpeed);
        AddToRotationBuffer(moveRotation);
        SetRotation();
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GridObject>() != null)
        {
            hasSnapped = false;
            nextBlock = other.GetComponent<GridObject>();
        }
        else if (other.GetComponent<ArenaWall>() != null)
        {
            snake.GetHit();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (hasSnapped == true)
        {
            return;
        }
        if (other.GetComponent<GridObject>() != null)
        {
            // ignore the y axis
            Vector3 gridBlockPosition = new Vector3(other.transform.position.x, 0f, other.transform.position.z);
            Vector3 nextGridBlockPosition = new Vector3(nextBlock.transform.position.x, 0f, nextBlock.transform.position.z);
            Vector3 snakeHeadPosition = new Vector3(transform.position.x, 0f, transform.position.z);

            Vector3 movementDirection = RotationToMovementVector(GetRotation());
            Vector3 directionToBlock = nextGridBlockPosition - transform.position;
            float dotProduct = Vector3.Dot(movementDirection, directionToBlock.normalized);
            // too small distance can cause the snake to not turn when needed
            if (Vector3.Distance(snakeHeadPosition, gridBlockPosition) <= 0.03f || dotProduct < 0) // dot product nam pove ali vektorja kažeta v isto ali nasprotno smer
            {
                if (rotationBuffer.Count > 0)
                {
                    // snap to the place of the grid block
                    transform.position = new Vector3(gridBlockPosition.x, transform.position.y, gridBlockPosition.z);
                    hasSnapped = true;

                    SetRotation();
                }
            }
        }
    }

    public void Move()
    {
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.forward); // Vector3.forward --> local space, transform.forward --> world space
    }

    public void SetRotation()
    {
        if (rotationBuffer.Count <= 0)
        {
            return;
        }

        transform.Rotate(0, rotationBuffer.First.Value, 0);
        transform.parent.GetComponent<Snake>().SetTorsoRotation(rotationBuffer.First.Value);
        rotationBuffer.RemoveFirst();
    }

    public void AddToRotationBuffer(float rotation)
    {
        //Debug.Log($"Dodaj v buffer: {rotation}");
        // to prevent spam
        if (rotationBuffer.Count == 2)
        {
            return;
        }
        rotationBuffer.AddLast(rotation);
    }

    public float GetRotation()
    {
        return transform.rotation.eulerAngles.y;
    }
    public bool isLast()
    {
        return lastSnakePart;
    }

    public void setLast()
    {
        lastSnakePart = true;
    }

    public void unsetLast()
    {
        lastSnakePart = false;
    }

    public Transform getTransform()
    {
        return transform;
    }

    public LinkedList<float> GetRotationBuffer()
    {
        return rotationBuffer;
    }
    public LinkedList<Vector3> GetPositionBuffer()
    {
        return new LinkedList<Vector3>();
    }

    public float GetNextRotation()
    {
        if (rotationBuffer.Count == 0)
        {
            return 0f;
        }
        return rotationBuffer.First.Value;
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
}
