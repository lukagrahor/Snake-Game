using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using UnityEngine.LowLevel;
using static UnityEngine.UI.GridLayoutGroup;

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
    //private float repathCooldown = 1.5f;
    //private float repathTimer = 0f;
    private float rotationSpeed = 400f;

    Vector3 targetPos = Vector3.zero;
    Vector3 npcPos = Vector3.zero;
    Vector3 moveDirection = Vector3.zero;
    private bool isRotating;
    float speed = 0.9f;
    private float rotationThreshold = 5f;
    Quaternion targetRotation = Quaternion.identity;
    //private bool pathExpired;
    private float idleWaitTime = 5f;
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
        CalculatePath();
        //Debug.Log("First path " + path[0]);
        targetPos = new Vector3(path[pathIndex].transform.position.x, 0f, path[pathIndex].transform.position.z);
        isRotating = false;
        pathCalculating = false;
        //PlayerActions.PlayerDeath += PlayerDied;
    }

    public void Update()
    {
        if (path == null || path.Count == 0)
        {

            /*Debug.Log("Bernard");
            Debug.Log("Bernard Count " + path.Count);
            Debug.Log("Bernard path " + path);*/
            Debug.Log("Bernard");
            CalculatePathAsync();
        }

        //repathTimer += Time.deltaTime;
        if (isRotating)
        {
            Rotate();
        }
        else
        {
            /*
            if (pathCalculating == false)
            {
                Debug.Log("Repath");
                CalculatePathAsync();
            }*/

            npcPos = new Vector3(npc.transform.position.x, 0f, npc.transform.position.z);
            float distance = Vector3.Distance(npcPos, targetPos);
            Vector3 moveDirection = (targetPos - npcPos).normalized;
            float dotProduct = Vector3.Dot(npc.transform.forward, moveDirection);
            //Debug.Log("distance:" + distance);
            //Debug.Log("dotProduct:" + dotProduct);
            if (distance <= 0.01f || (dotProduct < 0 && distance <= 0.1f))
            {
                SetNextPoint();
            }
            Move();
        }
    }

    void SetNextPoint()
    {
        Debug.Log("dot pathIndex:" + pathIndex);
        Debug.Log("dot pathBlock:" + path[pathIndex]);
        //npc.transform.position = new Vector3(targetPos.x, npc.transform.position.y, targetPos.z);
        pathIndex++;

        if (pathIndex >= path.Count || pathIndex < 0)
        {
            //Debug.Log("Bogdan nextBlock:" + npc.NextBlock.name);
            //stateMachine.idleState.WaitTime = idleWaitTime;
            stateMachine.TransitionTo(stateMachine.idleState);
        }

        targetPos = new Vector3(path[pathIndex].transform.position.x, 0f, path[pathIndex].transform.position.z);
        npcPos = new Vector3(npcPos.x, 0f, npcPos.z);

        moveDirection = Vector3.Normalize(targetPos - npcPos);
        targetRotation = RotateTowardsNextPoint(npcPos, targetPos);

        if (moveDirection.magnitude > 0.01f)
        {
            float targetAngleY = Mathf.Round(Quaternion.LookRotation(moveDirection, Vector3.up).eulerAngles.y / 90f) * 90f;
            targetRotation = Quaternion.Euler(0f, targetAngleY, 0f);
            isRotating = true;
        }
        else
        {
            isRotating = false;
        }

        if (isRotating) npc.transform.position = new Vector3(path[pathIndex - 1].transform.position.x, npc.transform.position.y, path[pathIndex - 1].transform.position.z);
    }
    void Rotate()
    {
        npc.transform.rotation = Quaternion.RotateTowards(npc.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (Quaternion.Angle(npc.transform.rotation, targetRotation) < rotationThreshold)
        {
            npc.transform.rotation = targetRotation;
            targetRotation = Quaternion.identity;
            isRotating = false;
        }
    }

    void Move()
    {
        //npc.transform.Translate(speed * Time.deltaTime * npc.transform.forward, Space.World);
        npc.transform.Translate(speed * Time.deltaTime * Vector3.forward, Space.Self);
    }
    /*
    void PlayerDied()
    {
        stopChasing = true;
    }
    */
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
    void CalculatePath()
    {
        // problem je, da ne pride do svoje prejšnje tarèe, ampak se kar zaène premikat po novi poti --> pade iz poti
        // upošteva se, da je že na zaèetki poti, lahko se pa zgodi, da ni
        List<GridObject> gridObjectsWithFood = grid.ObjectsWithFood;
        if (gridObjectsWithFood.Count <= 0) return;
        int randomIndex = Random.Range(0, gridObjectsWithFood.Count);
        GridObject foodPositionObject = gridObjectsWithFood[randomIndex];
        path = pathfinder.FindPath(npc.NextBlock, foodPositionObject);
        pathSpawner.RemoveMarkers();
        pathSpawner.SpawnMarkers(path);
        //repathTimer = 0;
        pathIndex = 0;
    }

    private async void CalculatePathAsync()
    {
        List<GridObject> gridObjectsWithFood = grid.ObjectsWithFood;
        if (gridObjectsWithFood.Count <= 0) return;
        pathCalculating = true;
        int randomIndex = Random.Range(0, gridObjectsWithFood.Count);
        GridObject foodPositionObject = gridObjectsWithFood[randomIndex];
        pathfindingTask = pathfinder.FindPathAsync(path[path.Count - 1], foodPositionObject);
        List<GridObject> newPath = await pathfindingTask;

        //newPath.RemoveAt(0); // ta prva toèka na novi poti je ta zadnja toèka na že obstojeèi poti
        foreach (GridObject newPathItem in newPath)
        {
            Debug.Log("newPathItem " + newPathItem.name);
        }
        path.AddRange(newPath);
        //Debug.Log("Gorazd path " + path.Count);
        pathSpawner.SpawnMarkers(newPath);
        //repathTimer = 0;
        pathCalculating = false;
    }

    public void Exit()
    {

    }

    public void HandleSnakeDeath()
    {
        throw new System.NotImplementedException();
    }
}
