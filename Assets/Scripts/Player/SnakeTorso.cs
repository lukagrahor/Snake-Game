using System;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTorso : MonoBehaviour, ISnakePart, IFrontTriggerHandler
{
    float moveSpeed = 0f;
    bool lastSnakePart = true;

    LinkedList<float> rotationBuffer;
    LinkedList<Vector3> positionBuffer;

    private bool hasSnapped = false;
    ISnakePart previousPart;
    Timer timer;

    bool hasTurned = false;
    float size;

    int konecJe = 1;

    public void HandleFrontTrigger()
    {
        Debug.Log($"Konec je! {konecJe}");
        konecJe++;
    }

    public void HandleTrigger(GridObject gridObject)
    {
        gridObject.IsOccupied = true;
        hasSnapped = false;
        //timer.StartTimer();
    }

    public void HandleTriggerExit(GridObject gridObject)
    {
        if (lastSnakePart == true)
        {
            gridObject.IsOccupied = false;
        }
    }
    void Awake()
    {
        rotationBuffer = new LinkedList<float>();
        positionBuffer = new LinkedList<Vector3>();
        timer = new Timer();
    }

    void Update()
    {
        Move();
    }

    public void AddToRotationBuffer(float rotation)
    {
        rotationBuffer.AddLast(rotation);
    }

    public void AddToPositionBuffer(Vector3 position)
    {
        positionBuffer.AddLast(position);
    }
    public void Move()
    {
        CheckForTurn();
  
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.forward);

        if (hasTurned == true)
        {
            FixGaps();
        }
    }

    public void Setup(float moveSpeed, float moveRotation, Snake snake, Vector3 snakeScaleVector)
    {
        this.moveSpeed = moveSpeed;

        transform.localPosition = new Vector3(0, 0, -1f);
        SetStartingRotation(moveRotation);

        transform.SetParent(snake.transform);
        transform.localScale = snakeScaleVector;
        size = snakeScaleVector.x;
    }

    private void CheckForTurn()
    {
        // ignore the y axis
        if (positionBuffer.Count == 0 || hasSnapped == true)
        {
            return;
        }
        Vector3 blockPositionWithY = positionBuffer.First.Value;

        Vector3 gridBlockPosition = new (blockPositionWithY.x, 0f, blockPositionWithY.z);
        Vector3 snakeTorsoPosition = new (transform.position.x, 0f, transform.position.z);
        float floatOffset = Vector3.Distance(snakeTorsoPosition, gridBlockPosition);
        
        Vector3 movementDirection = RotationToMovementVector(GetRotation());
        Vector3 directionToBlock = gridBlockPosition - transform.position;
        float dotProduct = Vector3.Dot(movementDirection, directionToBlock.normalized);

        // dot product nam pove ali vektorja kažeta v isto ali nasprotno smer
        if (floatOffset <= 0.03f || dotProduct < 0) 
        {
            if (rotationBuffer.Count > 0)
            {
                SetRotation();
                // na sredino grid kocke
                transform.position = new Vector3(gridBlockPosition.x, transform.position.y, gridBlockPosition.z);

                hasSnapped = true;
            }
        }
        
    }

    public void SetStartingRotation(float rotation)
    {
        transform.eulerAngles = new Vector3(0f, rotation, 0f);
    }

    public void SetRotation()
    {
        if (rotationBuffer.Count <= 0)
        {
            return;
        }

        transform.rotation = Quaternion.Euler(0, GetRotation() + rotationBuffer.First.Value, 0);

        //float timePassed = timer.StopTimer();
        //Debug.Log($"Torso: {gameObject.name}, timePassed: {timePassed}");

        rotationBuffer.RemoveFirst();
        positionBuffer.RemoveFirst();
        hasTurned = true;
    }

    void FixGaps()
    {
        if (rotationBuffer.Count != 0) { return; }
        Vector3 previous = previousPart.GetTransform().position;
        Vector3 newCurrentPosition = previous - (previousPart.GetTransform().forward * size);
        transform.position = newCurrentPosition;
    }

    public void SetPreviousPart(ISnakePart previousPart)
    {
        this.previousPart = previousPart;
    }
    public float GetRotation()
    {
        return transform.rotation.eulerAngles.y;
    }

    public void UnsetLast()
    {
        lastSnakePart = false;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void CopyBuffers (LinkedList<float> rotationBuffer, LinkedList<Vector3> positionBuffer)
    {
        foreach (var rotation in rotationBuffer)
        {
            this.rotationBuffer.AddLast(rotation);
        }

        foreach (var position in positionBuffer)
        {
            this.positionBuffer.AddLast(position);
        }
    }

    public LinkedList<float> GetRotationBuffer()
    {
        return rotationBuffer;
    }
    public LinkedList<Vector3> GetPositionBuffer()
    {
        return positionBuffer;
    }

    Vector3 RotationToMovementVector(float rotation)
    {
        // rotacije niso zmeraj tako kot bi si želel
        // 90.000001 --> pri rotaciji pride do float precision errors, zato zaokorðim
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
