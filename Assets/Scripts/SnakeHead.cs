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
    LinkedList<float> rotationBuffer;
    // for debugging
    int blocksPassed = 0;
    float time = 0f;
    Snake snake;
    GridObject nextBlock;

    void Awake()
    {
        rotationBuffer = new LinkedList<float>();
        //Debug.Log("Head attached");
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void Setup(float moveSpeed, float moveRotation, Transform parentTransform, Snake snake)
    {
        //onRotate += snake.SetTorsoRotation;
        this.snake = snake;
        transform.SetParent(parentTransform);

        SetMoveSpeed(moveSpeed);
        //Debug.Log($"moveSpeed: {moveSpeed}");
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
            //Debug.Log($"other {other.GetComponent<GridObject>().getId()}");
            time = Time.realtimeSinceStartup;
            hasSnapped = false;
            blocksPassed += 1;
            nextBlock = other.GetComponent<GridObject>();
        }
        else if (other.GetComponent<ArenaWall>() != null)
        {
            snake.GetHit();
        }
    }
    /*
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<GridObject>() != null)
        {
            //Debug.Log($"Izhod iz grid kocke: {other.GetComponent<GridObject>().getId()}");
        }
    }*/

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log($"Collision: {other.GetComponent<Food>()}");
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
            //Debug.Log($"Head movementDirection: {movementDirection}");
            //Debug.Log($"Head directionToBlock: {directionToBlock}");
            float dotProduct = Vector3.Dot(movementDirection, directionToBlock.normalized);
            //Debug.Log($"Head dotProduct: {dotProduct}");

            //Debug.Log($"Distance: {Vector3.Distance(snakeHeadPosition, gridBlockPosition)}");
            // too small distance can cause the snake to not turn when needed
            if (Vector3.Distance(snakeHeadPosition, gridBlockPosition) <= 0.03f || dotProduct < 0) // dot product nam pove ali vektorja kažeta v isto ali nasprotno smer
            {
                //Debug.Log($"rotationBuffer count: {rotationBuffer.Count}");
                //Debug.Log("Jabadabadu1");
                if (rotationBuffer.Count > 0)
                {
                    /*
                    Debug.Log($"Ime bloka: {other.GetComponent<GridObject>()}");
                    Debug.Log($"Head turn position: {gridBlockPosition}");
                    */
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
        //Debug.Log($"snakeheadRotation1: {transform.rotation.eulerAngles}");
        if (rotationBuffer.Count <= 0)
        {
            return;
        }

        //Debug.Log("Rotiraj!");
        //Debug.Log($"Prva rotacija v bufferju: {rotationBuffer.First.Value}");
        transform.Rotate(0, rotationBuffer.First.Value, 0);
        time = Time.realtimeSinceStartup - time;
        //Debug.Log($"Head speed: {moveSpeed}");
        //Debug.Log($"Èas potreben, da head doseže lokacijo rotacije: {time}");
        transform.parent.GetComponent<Snake>().SetTorsoRotation(rotationBuffer.First.Value);
        rotationBuffer.RemoveFirst();
        //transform.parent.GetComponent<Snake>().SetSnakeYRotation(GetRotation());
        //transform.parent.GetComponent<Snake>().SetNextTorsoRotation(GetRotation());
        

        /*if (onRotate != null)
        {
            onRotate();
        }*/

        //moveSpeed -= 0.03f;
        //Debug.Log($"blocksPassed: {blocksPassed}");

        //Debug.Log($"Prva rotacija v bufferju po brisanju: {rotationBuffer.First.Value}");

        //Debug.Log($"snakeheadRotation2: {transform.rotation.eulerAngles}");
        //moveRotationY = rotation;
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
        //moveSpeed += 0.03f;
        blocksPassed = 0;
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
