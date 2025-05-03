using System;
using UnityEngine;

public class DogPatrolState : IState
{
    float primaryDirection;
    float secondaryDirection;
    float speed = 1.5f;
    float rotationSpeed = 400f;
    bool isChangingLane = false;
    float rotationThreshold = 5f;
    Dog npc;
    SnakeHead player;
    DogStateMachine stateMachine;
    ArenaGrid grid;
    GridObject[,] gridObjects;
    GridObject currentBlock;
    GridObject nextBlock;
    GridObject lastLaneBlock;
    //float currentRotation = -1f;
    bool changedLane = false;
    bool isRotatingPrimary = false;
    bool isRotatingSecondary = false;

    enum Directions
    {
        Up = 0,
        Right = 90,
        Down = 180,
        Left = 270
    }

    public DogPatrolState(Dog npc, SnakeHead player, DogStateMachine stateMachine, ArenaGrid grid)
    {
        this.npc = npc;
        this.player = player;
        this.stateMachine = stateMachine;
        this.grid = grid;
        gridObjects = grid.GetGridObjects();
    }
    // doloèi ali pes gre gor dol al levo desno --> random

    public void Enter()
    {
        Debug.Log("Entered the patrol state");
        currentBlock = npc.StartBlock; // ko se menja state je treba to posodobit
        Array DirectionsValues = Enum.GetValues(typeof(Directions));
        int index = UnityEngine.Random.Range(0, DirectionsValues.Length);
        Directions randomDirection = (Directions)DirectionsValues.GetValue(index);
        primaryDirection = (float)randomDirection;
        Debug.Log("primaryDirection " + primaryDirection + " index " + index);

        if (randomDirection == Directions.Up || randomDirection == Directions.Down)
        {
            float[] secondaryDirections = { 90f, 270f };
            index = UnityEngine.Random.Range(0, 2);
            secondaryDirection = secondaryDirections[index];
            //if (secondaryDirection == 0f) LastLaneBlock = gridObjects[];
        }
        else if (randomDirection == Directions.Left || randomDirection == Directions.Right)
        {
            float[] secondaryDirections = { 0f, 180f };
            index = UnityEngine.Random.Range(0, 2);
            secondaryDirection = secondaryDirections[index];
        }

        Debug.Log("secondaryDirection " + secondaryDirection + " index " + index);
        npc.transform.eulerAngles = new Vector3(0f, primaryDirection, 0f);

        SetLastLaneBlock();
    }

    public void Update()
    {
        if (isRotatingPrimary || isRotatingSecondary)
        {
            //Debug.Log("Dog 1");
            Rotate();
            return;
        }

        if (isChangingLane) CheckForLaneStart();
        else CheckForLaneEnd();

        if (isRotatingPrimary || isRotatingSecondary) return;
        //Debug.Log("Dog 3");
        Move();
    }

    void Rotate()
    {
        float currentRotation;
        if (isRotatingPrimary) currentRotation = primaryDirection;
        else if (isRotatingSecondary) currentRotation = secondaryDirection;
        else return;

        Quaternion targetRotation = Quaternion.Euler(0f, currentRotation, 0f);
        npc.transform.rotation = Quaternion.RotateTowards(npc.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        SetNewLane();
        if (Quaternion.Angle(npc.transform.rotation, targetRotation) < rotationThreshold)
        {
            npc.transform.rotation = targetRotation;

            isRotatingPrimary = false;
            isRotatingSecondary = false;
            //currentBlock = npc.NextBlock;
            Debug.Log("Dog currentBlock " + currentBlock.name);
            if (changedLane)
            {
                SetLastLaneBlock();
                changedLane = false;
            }
        }
    }

    void ChangeLane()
    {
        if (isRotatingPrimary || isRotatingSecondary) return;
        Debug.Log("Menjej pas");
        //currentBlock = npc.NextBlock;
        //Debug.Log("Dog currentBlock " + currentBlock.name);
        isRotatingSecondary = true;
        isChangingLane = true;
    }

    void SetNewLane()
    {
        Debug.Log("Dog currentBlock " + currentBlock.name);
        int i = currentBlock.Row;
        int j = currentBlock.Col;
        Debug.Log("Dog prej i " + i + "j " + j);
        Debug.Log("secondaryDirection prej " + secondaryDirection);
        switch (secondaryDirection)
        {
            case (float)Directions.Up:
                i += 1;
                break;
            case (float)Directions.Down:
                i -= 1; 
                break;
            case (float)Directions.Left:
                j -= 1;
                break;
            case (float)Directions.Right:
                j += 1;
                break;
        }
        Debug.Log("Dog sredina i " + i + "j " + j);
        int gridSize = grid.GetSize();
        if (i < 0)
        {
            i = 1;
            secondaryDirection = (float)Directions.Up;
        }
        if (j < 0)
        {
            j = 1;
            secondaryDirection = (float)Directions.Right;
        }
        if (i > gridSize - 1)
        {
            i = gridSize - 2;
            secondaryDirection = (float)Directions.Down;
        }
        if (j > gridSize - 1)
        {
            j = gridSize - 2;
            secondaryDirection = (float)Directions.Left;
        }

        Debug.Log("secondaryDirection pol " + secondaryDirection);
        nextBlock = gridObjects[j, i];
        Debug.Log("Dog pol i " + i + "j " + j);
        Debug.Log("Dog nextBlock " + nextBlock.name);
    }

    void SetLastLaneBlock()
    {
        int i = currentBlock.Row;
        int j = currentBlock.Col;
        int laneSize = grid.GetSize(); 
        switch (primaryDirection)
        {
            case (float)Directions.Up:
                i = laneSize - 1;
                break;
            case (float)Directions.Down:
                i = 0;
                break;
            case (float)Directions.Left:
                j = 0;
                break;
            case (float)Directions.Right:
                j = laneSize - 1;
                break;
        }
        lastLaneBlock = gridObjects[j, i];
        Debug.Log("Dog lastLaneBlock " + lastLaneBlock.name);
    }

    void CheckForLaneStart()
    {
        if (nextBlock == null) return;
        Debug.Log("CheckForLaneStart");
        Vector3 npcPos = new Vector3(npc.transform.position.x, 0f, npc.transform.position.z);
        Vector3 nextBlockPos = new Vector3(nextBlock.transform.position.x, 0f, nextBlock.transform.position.z);
        float distance = Vector3.Distance(npcPos, nextBlockPos);
        Vector3 moveDirection = (nextBlockPos - npcPos).normalized;
        float dotProduct = Vector3.Dot(npc.transform.forward, moveDirection);
        //Debug.Log("distance:" + distance);
        //Debug.Log("dotProduct:" + dotProduct);
        if (distance <= 0.01f || (dotProduct < 0 && distance <= 0.1f))
        {
            npc.transform.position = new Vector3(nextBlockPos.x, npc.transform.position.y, nextBlockPos.z);

            float currentRotation = primaryDirection;
            if (currentRotation - 180f >= 0)
            {
                primaryDirection -= 180f;
            }
            else
            {
                primaryDirection += 180f;
            }

            isChangingLane = false;
            changedLane = true;
            isRotatingPrimary = true;
            currentBlock = nextBlock;
            Debug.Log("Dog lane start reached");
            Debug.Log("Dog currentRotation " + currentRotation);
        }
    }

    void CheckForLaneEnd()
    {
        //Debug.Log("Dog CheckForLaneEnd");
        if (lastLaneBlock == null)
        {
            Debug.Log("Dog error lastLaneBlock is null");
            return;
        }
        
        Vector3 npcPos = new Vector3(npc.transform.position.x, 0f, npc.transform.position.z);
        Vector3 lastLaneBlockPos = new Vector3(lastLaneBlock.transform.position.x, 0f, lastLaneBlock.transform.position.z);
        float distance = Vector3.Distance(npcPos, lastLaneBlockPos);
        Vector3 moveDirection = (lastLaneBlockPos - npcPos).normalized;
        float dotProduct = Vector3.Dot(npc.transform.forward, moveDirection);
        //Debug.Log("distance:" + distance);
        //Debug.Log("dotProduct:" + dotProduct);
        if (distance <= 0.01f || (dotProduct < 0 && distance <= 0.1f))
        {
            npc.transform.position = new Vector3(lastLaneBlockPos.x, npc.transform.position.y, lastLaneBlockPos.z);
            currentBlock = lastLaneBlock;
            ChangeLane();
        }
        
    }

    void Move()
    {
        npc.transform.Translate(speed * Time.deltaTime * Vector3.forward);
    }

    public void Exit()
    {

    }
}