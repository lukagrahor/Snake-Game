using System;
using UnityEngine;

public class DogPatrolState : IState
{
    float primaryDirection;
    float secondaryDirection;
    float speed = 1.25f;
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
        if (npc == null) return;
        currentBlock = npc.StartBlock;
        Array DirectionsValues = Enum.GetValues(typeof(Directions));
        int index = UnityEngine.Random.Range(0, DirectionsValues.Length);
        Directions randomDirection = (Directions)DirectionsValues.GetValue(index);
        primaryDirection = (float)randomDirection;

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

        npc.transform.eulerAngles = new Vector3(0f, primaryDirection, 0f);

        SetLastLaneBlock();
    }

    public void Update()
    {
        if (npc == null) return;
        if (isRotatingPrimary || isRotatingSecondary)
        {
            Rotate();
            return;
        }

        if (isChangingLane) CheckForLaneStart();
        else CheckForLaneEnd();

        if (isRotatingPrimary || isRotatingSecondary) return;
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
            if (changedLane)
            {
                SetLastLaneBlock();
                changedLane = false;
            }
        }
    }

    public void ChangeLane()
    {
        if (isRotatingPrimary || isRotatingSecondary) return;
        isRotatingSecondary = true;
        isChangingLane = true;
    }

    void SetNewLane()
    {
        // tukej moram tudi upoštevat zide
        int i = lastLaneBlock.Row;
        int j = lastLaneBlock.Col;
        Debug.Log("SetNewLane before:" + "Row: " + i + " Col: " + j);
        int gridSize = grid.GetSize();
        switch (secondaryDirection)
        {
            case (float)Directions.Up:
                i += 1;
                if (i >= gridSize) break;
                if (gridObjects[j, i].IsOccupiedByWall)
                {
                    i -= 2;
                    secondaryDirection = (float)Directions.Down;
                }
                break;
            case (float)Directions.Down:
                i -= 1;
                if (i < 0) break;
                if (gridObjects[j, i].IsOccupiedByWall)
                {
                    i+= 2;
                    secondaryDirection = (float)Directions.Up;
                }
                break;
            case (float)Directions.Left:
                j -= 1;
                if (j < 0) break;
                if (gridObjects[j, i].IsOccupiedByWall)
                {
                    j += 2;
                    secondaryDirection = (float)Directions.Right;
                }
                break;
            case (float)Directions.Right:
                j += 1;
                if (j >= gridSize) break;
                if (gridObjects[j, i].IsOccupiedByWall)
                {
                    j -= 2;
                    secondaryDirection = (float)Directions.Left;
                }
                break;
        }

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

        Debug.Log("SetNewLane after:" + "Row: " + i + " Col: " + j);
        nextBlock = gridObjects[j, i];
    }

    void SetLastLaneBlock()
    {
        int i = currentBlock.Row;
        int j = currentBlock.Col;
        int laneSize = grid.GetSize();
        Debug.Log("SetLastLaneBlock");
        switch (primaryDirection)
        {
            // preverimo ali je v stolpcu/vrstici kakšen zid
            case (float)Directions.Up:
                for (int index = i; index < laneSize; index++)
                {
                    if (gridObjects[j, index].IsOccupiedByWall) break;
                    i = index;
                }
                break;
            case (float)Directions.Down:
                for (int index = i; index >= 0; index--)
                {
                    if (gridObjects[j, index].IsOccupiedByWall) break;
                    i = index;
                }
                break;
            case (float)Directions.Left:
                for (int index = j; index >= 0; index--)
                {
                    if (gridObjects[index, i].IsOccupiedByWall) break;
                    j = index;
                }
                break;
            case (float)Directions.Right:
                for (int index = j; index < laneSize; index++)
                {
                    if (gridObjects[index, i].IsOccupiedByWall) break;
                    j = index;
                }
                break;
        }
        lastLaneBlock = gridObjects[j, i];
    }

    void CheckForLaneStart()
    {
        if (nextBlock == null) return;
        Vector3 npcPos = new Vector3(npc.transform.position.x, 0f, npc.transform.position.z);
        Vector3 nextBlockPos = new Vector3(nextBlock.transform.position.x, 0f, nextBlock.transform.position.z);
        float distance = Vector3.Distance(npcPos, nextBlockPos);
        Vector3 moveDirection = (nextBlockPos - npcPos).normalized;
        float dotProduct = Vector3.Dot(npc.transform.forward, moveDirection);
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
        }
    }

    void CheckForLaneEnd()
    {
        if (lastLaneBlock == null)
        {
            return;
        }
        
        Vector3 npcPos = new Vector3(npc.transform.position.x, 0f, npc.transform.position.z);
        Vector3 lastLaneBlockPos = new Vector3(lastLaneBlock.transform.position.x, 0f, lastLaneBlock.transform.position.z);
        float distance = Vector3.Distance(npcPos, lastLaneBlockPos);
        Vector3 moveDirection = (lastLaneBlockPos - npcPos).normalized;
        float dotProduct = Vector3.Dot(npc.transform.forward, moveDirection);
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