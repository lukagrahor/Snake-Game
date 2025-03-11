using System;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTorso : MonoBehaviour, ISnakePart
{
    float moveSpeed = 0f;
    float size = 0.4f;
    bool lastSnakePart = true;

    LinkedList<float> rotationBuffer;
    LinkedList<Vector3> positionBuffer;

    private bool hasSnapped = false;
    ISnakePart previousPart;
    Timer timer;
    void Awake()
    {
        rotationBuffer = new LinkedList<float>();
        positionBuffer = new LinkedList<Vector3>();
        timer = new Timer();
    }

    void FixedUpdate()
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
    }

    public void Setup(float moveSpeed, float moveRotation, Transform snakeTransform)
    {
        this.moveSpeed = moveSpeed;

        transform.localPosition = new Vector3(0, 0, -1f);
        SetStartingRotation(moveRotation);

        transform.SetParent(snakeTransform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GridObject>() != null)
        {
            timer.StartTimer();
            hasSnapped = false;
        }
    }

    private void CheckForTurn()
    {
        // ignore the y axis
        if (positionBuffer.Count == 0 || hasSnapped == true)
        {
            return;
        }
        Vector3 blockPositionWithY = positionBuffer.First.Value;

        Vector3 gridBlockPosition = new Vector3(blockPositionWithY.x, 0f, blockPositionWithY.z);
        Vector3 snakeTorsoPosition = new Vector3(transform.position.x, 0f, transform.position.z);
        float floatOffset = Vector3.Distance(snakeTorsoPosition, gridBlockPosition);
        
        

        Vector3 movementDirection = RotationToMovementVector(GetRotation());
        Vector3 directionToBlock = gridBlockPosition - transform.position;
        float dotProduct = Vector3.Dot(movementDirection, directionToBlock.normalized);

        // dot product nam pove ali vektorja ka�eta v isto ali nasprotno smer
        if (floatOffset <= 0.03f || dotProduct < 0) 
        {
            Debug.Log($"Torso: {gameObject.name}, floatOffset: {floatOffset}");
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
    }
    public void SetPreviousPart(ISnakePart previousPart)
    {
        this.previousPart = previousPart;
    }
    public float GetRotation()
    {
        return transform.rotation.eulerAngles.y;
    }
    public bool IsLast()
    {
        return lastSnakePart;
    }

    public void SetLast()
    {
        lastSnakePart = true;
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
        // rotacije niso zmeraj tako kot bi si �elel
        // 90.000001 --> pri rotaciji pride do float precision errors, zato zaokor�im
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
