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
    float currentRotation = -1f;

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
        if (currentRotation >= 0f)
        {
            //Debug.Log("Dog 1");
            Rotate();
            return;
        }

        if (isChangingLane) CheckForLaneStart();
        else CheckForLaneEnd();

        //Debug.Log("Dog 3");
        Move();
    }

    void Rotate()
    {
        Quaternion targetRotation = Quaternion.Euler(0f, secondaryDirection, 0f);
        npc.transform.rotation = Quaternion.RotateTowards(npc.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        if (Quaternion.Angle(npc.transform.rotation, targetRotation) < rotationThreshold)
        {
            npc.transform.rotation = targetRotation;
            targetRotation = Quaternion.identity;
            SetNewLane();
            currentRotation = -1f;
            //SetLastLaneBlock();
        }
    }

    void ChangeLane()
    {
        if (currentRotation > 0f) return;
        Debug.Log("Menjej pas");
        currentRotation = secondaryDirection;
        currentBlock = npc.NextBlock;
        Debug.Log("Dog currentBlock " + currentBlock.name);
        isChangingLane = true;
    }

    void SetNewLane()
    {
        int i = currentBlock.Row;
        int j = currentBlock.Col;
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
        nextBlock = gridObjects[j, i];
        Debug.Log("Dog i " + i + "j " + j);
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
        //Debug.Log("Dog 2");
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

            currentRotation = primaryDirection;
            if (currentRotation - 180f >= 0)
            {
                currentRotation -= 180f;
            } else
            {
                currentRotation += 180f;
            }

            isChangingLane = false;
            Debug.Log("Dog lane start reached");
        }
    }

    void CheckForLaneEnd()
    {
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