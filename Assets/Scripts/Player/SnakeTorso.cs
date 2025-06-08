using System;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTorso : MonoBehaviour, ISnakePart, IFrontTriggerHandler, IWaspFrontTriggerHandler, IGridObjectStayTriggerHandler
{
    [SerializeField] MeshRenderer torsoRenderer;
    public float MoveSpeed { get; set; }
    bool lastSnakePart = true;

    LinkedList<float> rotationBuffer;
    LinkedList<Vector3> positionBuffer;

    //private bool hasSnapped = false;
    Snake snake;
    ISnakePart previousPart;
    Timer timer;

    bool hasTurned = false;
    float size;
    bool stop = false;

    private void OnTriggerEnter(Collider other)
    {
        ISnakeTorsoTriggerHandler enteredObject = other.GetComponent<ISnakeTorsoTriggerHandler>();
        enteredObject?.HandleTorsoTrigger(this);
    }

    public void HandleFrontTrigger()
    {
        if (gameObject.name == "Torso 0" || gameObject.name == "Torso 1")
        {
            return;
        }
        snake.GetHit();
    }

    public void HandleTrigger(GridObject gridObject)
    {
        //gridObject.IsOccupied = true;
        //gridObject.SetMarker();
        //hasSnapped = false;
        //timer.StartTimer();
    }

    public void HandleTrigger(Wasp wasp)
    {
        WaspStateMachine stateMachine = wasp.Ai.waspStateMachine;
        if (stateMachine.CurrentState == stateMachine.ChargeState)
        {
            stateMachine.ChargeState.CoolDown();
        }
    }

    public void HandleTriggerExit(GridObject gridObject)
    {
        if (lastSnakePart == true)
        {
            if (gridObject == null) Debug.Log("Tanzanija");
            gridObject.IsOccupied = false;
            gridObject.RemoveMarker();
            gridObject.StopHeadTimer();
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
        if (stop == true)
        {
            return;
        }
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

    public void Stop()
    {
        stop = true;
    }

    public void Move()
    {
        //CheckForTurn();
  
        transform.Translate(MoveSpeed * Time.deltaTime * Vector3.forward);

        if (hasTurned == true)
        {
            FixGaps();
        }
    }

    public void Setup(float moveSpeed, float moveRotation, Snake snake, Vector3 snakeScaleVector)
    {
        this.MoveSpeed = moveSpeed;

        transform.localPosition = new Vector3(0, 0, -1f);
        SetStartingRotation(moveRotation);

        transform.SetParent(snake.transform);
        transform.localScale = snakeScaleVector;
        size = snakeScaleVector.x;

        this.snake = snake;
    }
    /*
    private void CheckForTurn()
    {
        // ignore the y axis
        // if (positionBuffer.Count == 0 || hasSnapped == true)
        if (positionBuffer.Count == 0)
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
                //AddMarkerToPath(gridBlockPosition, rotationBuffer.First.Value);
                SetRotation();
                AddMarkerToPath(gridBlockPosition, 0f);
                // na sredino grid kocke
                transform.position = new Vector3(gridBlockPosition.x, transform.position.y, gridBlockPosition.z);

                // hasSnapped = true;
            }
        }
        
    }*/

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
        hasTurned = false;
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
    void AddMarkerToPath(GridObject gridObject, Vector3 position, float rotation)
    {
        if (gameObject.name != "Torso 0") return;
        snake.SetSnakePath(gridObject, position, rotation);
    }

    public void HandleStayTrigger(GridObject gridObject)
    {
        if (positionBuffer.Count > 0) CheckForTurn(gridObject);
        if (gameObject.name == "Torso 0") CheckForMarker(gridObject);
    }

    void CheckForTurn(GridObject gridObject)
    {
        Vector3 blockPositionWithY = positionBuffer.First.Value;

        Vector3 gridBlockPosition = new(blockPositionWithY.x, 0f, blockPositionWithY.z);
        Vector3 currentBlockPosition = new(gridObject.transform.position.x, 0f, gridObject.transform.position.z);

        if (gridBlockPosition != currentBlockPosition) return; // zbriši to èe se pojavijo kakšne težave. Namenjeno je za to, da se izvede koda na tem bloku le èe je ta blok v position bufferju

        Vector3 snakeTorsoPosition = new(transform.position.x, 0f, transform.position.z);
        float floatOffset = Vector3.Distance(snakeTorsoPosition, gridBlockPosition);

        Vector3 movementDirection = RotationToMovementVector(GetRotation());
        Vector3 directionToBlock = gridBlockPosition - transform.position;
        float dotProduct = Vector3.Dot(movementDirection, directionToBlock.normalized);

        // dot product nam pove ali vektorja kažeta v isto ali nasprotno smer
        if (floatOffset <= 0.03f || (dotProduct < 0 && floatOffset <= 0.1f))
        {
            if (rotationBuffer.Count > 0)
            {
                if (gameObject.name == "Torso 0" && gridObject.HasPathMarker == false)
                {
                    AddMarkerToPath(gridObject, gridBlockPosition, rotationBuffer.First.Value);
                    gridObject.HasPathMarker = true;
                } else if (gameObject.name == "Torso 0" && gridObject.HasPathMarker == true)
                {
                    gridObject.Marker.NextRotation = rotationBuffer.First.Value;
                }
                SetRotation();
                // na sredino grid kocke
                transform.position = new Vector3(gridBlockPosition.x, transform.position.y, gridBlockPosition.z);
            }
        }
    }

    void CheckForMarker(GridObject gridObject)
    {
        if (gridObject.HasPathMarker == true) return;
        Vector3 gridBlockPosition = new(gridObject.transform.position.x, 0f, gridObject.transform.position.z);
        Vector3 snakeTorsoPosition = new(transform.position.x, 0f, transform.position.z);
        float floatOffset = Vector3.Distance(snakeTorsoPosition, gridBlockPosition);

        Vector3 movementDirection = RotationToMovementVector(GetRotation());
        Vector3 directionToBlock = gridBlockPosition - transform.position;
        float dotProduct = Vector3.Dot(movementDirection, directionToBlock.normalized);
        if (floatOffset <= 0.03f || (dotProduct < 0 && floatOffset <= 0.1f))
        {
            AddMarkerToPath(gridObject, gridBlockPosition, 0f);
            gridObject.HasPathMarker = true;
        }
    }

    public void GetHit()
    {
        snake.GetHit();
    }
    public void SetToTransparent()
    {
        Debug.Log("Transparentnost je kljuè do");
        Color tempColor = torsoRenderer.material.color;
        tempColor.a = 0.7f;
        torsoRenderer.material.color = tempColor;
    }

    public void SetToSolid()
    {
        Color tempColor = torsoRenderer.material.color;
        tempColor.a = 1f;
        torsoRenderer.material.color = tempColor;
    }
}
