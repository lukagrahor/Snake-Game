using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEngine.UI.GridLayoutGroup;

public class PursueState : IState
{
    protected ChaseEnemy npc;
    protected SnakeHead player;
    protected StateMachine stateMachine;
    protected PathSpawner pathSpawner;
    FindPathAStar pathfinder;
    List<Vector3> path;
    int pathIndex;
    private float repathCooldown = 1.5f;
    private float repathTimer = 0f;
    private float rotationSpeed = 400f;

    Vector3 targetPos = Vector3.zero;
    Vector3 npcPos = Vector3.zero;
    Vector3 moveDirection = Vector3.zero;
    private bool isRotating;
    float speed = 0.9f;
    private float rotationThreshold = 5f;
    Quaternion targetRotation = Quaternion.identity;
    private bool pathExpired;
    private bool stopChasing;
    private float idleWaitTime = 5f;

    public PursueState(ChaseEnemy npc, SnakeHead player, StateMachine stateMachine, ArenaGrid grid, PathSpawner pathSpawner)
    {
        this.npc = npc;
        this.player = player;
        this.stateMachine = stateMachine;
        this.pathSpawner = pathSpawner;
        pathfinder = new FindPathAStar(grid);
    }
    public void Enter()
    {
        /*
        CalculatePath();

        npcPos = new Vector3(npc.transform.position.x, 0f, npc.transform.position.z);
        targetPos = new Vector3(path[pathIndex].x, 0f, path[pathIndex].z);

        targetRotation = RotateTowardsNextPoint(npcPos, targetPos);
        npc.transform.rotation = targetRotation;
        //Debug.Log("dot npc.transform.forward: " + npc.transform.forward);
        isRotating = false;
        */
        CalculatePath();
        targetPos = new Vector3(path[pathIndex].x, 0f, path[pathIndex].z);
        isRotating = false;
        stopChasing = false;
        pathExpired = false;
        PlayerActions.PlayerDeath += PlayerDied;
    }
    public void Update()
    {
        if (pathIndex >= path.Count || path == null || path.Count == 0)
        {
            CalculatePath();
            repathTimer = 0f;
        }

        repathTimer += Time.deltaTime;
        if (isRotating)
        {
            Rotate();
        }
        else
        {
            if (repathTimer >= repathCooldown && pathExpired)
            {
                CalculatePath(); 
                if (path.Count > 0)
                {
                    targetPos = new Vector3(path[pathIndex].x, 0f, path[pathIndex].z);
                    targetRotation = RotateTowardsNextPoint(npcPos, targetPos);
                    isRotating = true;
                }
                pathExpired = false;
                return;
            }
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
        if (stopChasing)
        {
            stateMachine.idleState.WaitTime = idleWaitTime;
            stateMachine.TransitionTo(stateMachine.idleState);
        }
        //npc.transform.position = new Vector3(targetPos.x, npc.transform.position.y, targetPos.z);
        pathIndex++;
        if (pathIndex >= path.Count) return;

        targetPos = new Vector3(path[pathIndex].x, 0f, path[pathIndex].z);
        npcPos = new Vector3(npcPos.x, 0f, npcPos.z);
        moveDirection = Vector3.Normalize(targetPos - npcPos);

        if (repathTimer >= repathCooldown)
        {
            pathExpired = true;
            return;
        }

        targetRotation = RotateTowardsNextPoint(npcPos, targetPos);
        if (targetRotation != npc.transform.rotation) isRotating = true;
        if (isRotating) npc.transform.position = new Vector3(path[pathIndex - 1].x, npc.transform.position.y, path[pathIndex - 1].z);
    }

    void Rotate()
    {
        npc.transform.rotation = Quaternion.RotateTowards(npc.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (Quaternion.Angle(npc.transform.rotation, targetRotation) < rotationThreshold)
        {
            Debug.Log("targetRotation: " + targetRotation);
            npc.transform.rotation = targetRotation;
            targetRotation = Quaternion.identity;
            //Debug.Log("dot Zarotirano");
            isRotating = false;
        }
    }

    void Move()
    {
        //npc.transform.Translate(speed * Time.deltaTime * npc.transform.forward, Space.World);
        npc.transform.Translate(speed * Time.deltaTime * Vector3.forward, Space.Self);
    }

    void PlayerDied()
    {
        stopChasing = true;
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

    /*
    Quaternion RotateTowardsNextPoint(Vector3 currentPoint, Vector3 nextPoint)
    {
        if (path == null || path.Count <= pathIndex) return Quaternion.identity;

        Vector3 currentForward = npc.transform.forward;
        currentForward.y = 0f;
        currentForward.Normalize();

        Vector3 directionToNext = (nextPoint - currentPoint).normalized;
        directionToNext.y = 0f;
        directionToNext.Normalize();

        // Use the cross product to determine left or right
        Vector3 crossProduct = Vector3.Cross(currentForward, directionToNext);
        //Debug.Log("dot crossProduct: " + crossProduct);
        Quaternion newTargetRotation = npc.transform.rotation;

        Debug.Log("dot currentForward:" + currentForward);
        Debug.Log("dot directionToNext:" + directionToNext);
        Debug.Log("dot crossProduct:" + crossProduct);

        if (crossProduct.y > 0.13f) // Next point is to the left (cross product points up)
        {
            newTargetRotation *= Quaternion.Euler(0f, 90f, 0f);
            isRotating = true;
        }
        else if (crossProduct.y < -0.13f) // Next point is to the right (cross product points down)
        {
            newTargetRotation *= Quaternion.Euler(0f, -90f, 0f);
            isRotating = true;
        }
        else
        {
            float angle = Vector3.Angle(currentForward, directionToNext);
            if (angle > 5f)
            {
                newTargetRotation = Quaternion.LookRotation(directionToNext, Vector3.up);
                isRotating = true;
            }
        }

        Debug.Log("dot Current Forward: " + currentForward);
        Debug.Log("dot Direction to Next: " + directionToNext);
        Debug.Log("dot Cross Product Y: " + crossProduct.y);
        Debug.Log("dot Angle: " + Vector3.Angle(currentForward, directionToNext));


        return newTargetRotation;
    }
    */
    void CalculatePath()
    {
        // problem je, da ne pride do svoje prejšnje tarèe, ampak se kar zaène premikat po novi poti --> pade iz poti
        // upošteva se, da je že na zaèetki poti, lahko se pa zgodi, da ni
        path = pathfinder.FindPath(npc.NextBlock, player.GetNextBlock());
        pathSpawner.SpawnMarkers(path);
        pathIndex = 0;
        repathTimer = 0;
    }

    public void Exit()
    {

    }

    public void HandleSnakeDeath()
    {
        throw new System.NotImplementedException();
    }
}
