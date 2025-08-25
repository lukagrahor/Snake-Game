using System.Collections.Generic;
using UnityEngine;
using System;

public class FlyPursueState : IState
{
    protected Fly npc;
    protected SnakeHead player;
    protected FlyStateMachine stateMachine;
    protected ArenaGrid grid;
    protected PathSpawner pathSpawner;
    FindPathAStar pathfinder;
    List<GridObject> path;
    int pathIndex;
    private float rotationSpeed = 200f;

    Vector3 targetPos = Vector3.zero;
    Vector3 npcPos = Vector3.zero;
    Vector3 moveDirection = Vector3.zero;
    float speed = 0.7f;
    private float rotationThreshold = 5f;
    Quaternion targetRotation = Quaternion.identity;
    private Awaitable<List<GridObject>> pathfindingTask;
    private bool pathCalculating = false;

    public FlyPursueState(Fly npc, SnakeHead player, FlyStateMachine stateMachine, ArenaGrid grid, PathSpawner pathSpawner)
    {
        this.npc = npc;
        this.player = player;
        this.stateMachine = stateMachine;
        this.grid = grid;
        this.pathSpawner = pathSpawner;
        pathfinder = new FindPathAStar(grid);
    }
    public void Enter()
    {
        CalculatePathAsync();
        npc.IsRotating = false;
        pathCalculating = true;
        FoodActions.EatenByPlayer += CalculatePathAsync;
    }

    public void Update()
    {
        if (pathCalculating)
        {
            return;
        }
        if (path == null || path.Count == 0)
        {
            CalculatePathAsync();
            return;
        }

        if (npc.IsRotating)
        {
            Rotate();
        }
        else
        {
            npcPos = new Vector3(npc.transform.position.x, 0f, npc.transform.position.z);
            float distance = Vector3.Distance(npcPos, targetPos);
            Vector3 moveDirection = (targetPos - npcPos).normalized;
            float dotProduct = Vector3.Dot(npc.transform.forward, moveDirection);

            if (distance <= 0.01f || (dotProduct < 0 && distance <= 0.1f))
            {
                SetNextPoint();
            }

            if (distance > 1.7f)
            {
                CalculatePathAsync();
                return;
            }
            Move();
        }
    }

    void SetNextPoint()
    {
        pathIndex++;

        if (path == null || path.Count == 0)
        {
            return;
        }

        if (pathIndex >= path.Count || pathIndex < 0)
        {
            stateMachine.TransitionTo(stateMachine.idleState);
            return;
        }
        try
        {
            targetPos = new Vector3(path[pathIndex].transform.position.x, 0f, path[pathIndex].transform.position.z);
            npcPos = new Vector3(npcPos.x, 0f, npcPos.z);

            moveDirection = Vector3.Normalize(targetPos - npcPos);
            targetRotation = RotateTowardsNextPoint(npcPos, targetPos);

            if (moveDirection.magnitude > 0.01f)
            {
                float targetAngleY = Mathf.Round(Quaternion.LookRotation(moveDirection, Vector3.up).eulerAngles.y / 90f) * 90f;
                targetRotation = Quaternion.Euler(0f, targetAngleY, 0f);
                npc.IsRotating = true;
            }
            else
            {
                npc.IsRotating = false;
            }

            if (npc.IsRotating) npc.transform.position = new Vector3(path[pathIndex - 1].transform.position.x, npc.transform.position.y, path[pathIndex - 1].transform.position.z);
        } catch(ArgumentOutOfRangeException e)
        {
            Debug.LogException(e);
            Debug.Log("Ven iz obsega izjema"); // nit to vir težave
            path = new List<GridObject>();
            return;
        }
    }
    void Rotate()
    {
        npc.transform.rotation = Quaternion.RotateTowards(npc.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (Quaternion.Angle(npc.transform.rotation, targetRotation) < rotationThreshold)
        {
            npc.transform.rotation = targetRotation;
            targetRotation = Quaternion.identity;
            npc.IsRotating = false;
        }
    }

    void Move()
    {
        //npc.transform.Translate(speed * Time.deltaTime * npc.transform.forward, Space.World);
        npc.transform.Translate(speed * Time.deltaTime * Vector3.forward, Space.Self);
    }
    Quaternion RotateTowardsNextPoint(Vector3 currentPoint, Vector3 nextPoint)
    {
        if (path == null || path.Count <= pathIndex) return Quaternion.identity;

        Vector3 currentForward = npc.transform.forward;
        currentForward.y = 0f;
        currentForward.Normalize();

        Vector3 directionToNext = (nextPoint - currentPoint).normalized;
        directionToNext.y = 0f;

        Quaternion newTargetRotation = Quaternion.LookRotation(directionToNext, Vector3.up);

        return newTargetRotation;
    }
    /* prva kocka je kocka na kateri nasprotnik trenutno stoji */
    private async void CalculatePathAsync()
    {
        List<GridObject> gridObjectsWithFood = grid.ObjectsWithFood;
        if (gridObjectsWithFood.Count <= 0)
        {
            return;
        }
        pathCalculating = true;
        int randomIndex = UnityEngine.Random.Range(0, gridObjectsWithFood.Count);
        GridObject foodPositionObject = gridObjectsWithFood[randomIndex];
        pathfindingTask = pathfinder.FindPathAsync(npc.NextBlock, foodPositionObject);
        Debug.Log("Muha kuha. Še kalkuliram pot 1");
        List<GridObject> newPath = await pathfindingTask;
        Debug.Log("Muha kuha. Še kalkuliram pot 2");
        path = newPath;
        if (path.Count == 0)
        {
            return;
        }

        pathIndex = 0;
        targetPos = new Vector3(path[pathIndex].transform.position.x, 0f, path[pathIndex].transform.position.z);
        pathCalculating = false;
    }

    public void Exit()
    {
        pathfinder.CancelCurrentPathfinding();
    }

    public void HandleSnakeDeath()
    {
        
    }
}
