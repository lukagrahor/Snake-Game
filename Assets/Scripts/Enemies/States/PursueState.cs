using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class PursueState : IState
{
    protected ChaseEnemy npc;
    protected SnakeHead player;
    protected StateMachine stateMachine;
    protected PathSpawner pathSpawner;
    FindPathAStar pathfinder;
    List<Vector3> path;
    int pathIndex;
    private float repathCooldown = 0.5f;
    private float repathTimer = 0f;
    private float rotationSpeed = 360f;

    Vector3 targetPos = Vector3.zero;
    Vector3 npcPos = Vector3.zero;
    Vector3 moveDirection = Vector3.zero;
    private bool isRotating = false;
    private float speed = 0.3f;
    private float rotationThreshold = 5f;
    Quaternion targetRotation = Quaternion.identity;

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
        CalculatePath();
        pathSpawner.SpawnMarkers(path);

        npcPos = new Vector3(npc.transform.position.x, 0f, npc.transform.position.z);
        targetPos = new Vector3(path[pathIndex].x, 0f, path[pathIndex].z);

        targetRotation = RotateTowardsNextPoint(npcPos, targetPos);
        npc.transform.rotation = targetRotation;
        Debug.Log("dot npc.transform.forward: " + npc.transform.forward);
        isRotating = false;
    }
    public void Update()
    {
        if (path == null || path.Count == 0 || pathIndex >= path.Count) return;
        if (isRotating)
        {
            Rotate();
        }
        else
        {
            npcPos = new Vector3(npc.transform.position.x, 0f, npc.transform.position.z);
            float distance = Vector3.Distance(npcPos, targetPos);
            Vector3 moveDirection = (targetPos - npcPos).normalized;
            float dotProduct = Vector3.Dot(npc.transform.forward, moveDirection);
            Debug.Log("dot produkt veselja: " + dotProduct);
            if (distance <= 0.03f || (dotProduct < 0 && distance <= 0.1f))
            {
                Debug.Log("dot Sm blizi:" + targetPos);
                //npc.transform.position = new Vector3(targetPos.x, npc.transform.position.y, targetPos.z);
                pathIndex++;

                targetPos = new Vector3(path[pathIndex].x, 0f, path[pathIndex].z);
                npcPos = new Vector3(npcPos.x, 0f, npcPos.z);
                moveDirection = Vector3.Normalize(targetPos - npcPos);

                targetRotation = RotateTowardsNextPoint(npcPos, targetPos);
                if(targetRotation != npc.transform.rotation) isRotating = true;
            }
            Move();
        }
    }

    void Rotate()
    {
        npc.transform.rotation = Quaternion.RotateTowards(npc.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (Quaternion.Angle(npc.transform.rotation, targetRotation) < rotationThreshold)
        {
            npc.transform.rotation = targetRotation;
            targetRotation = Quaternion.identity;
            Debug.Log("dot Zarotirano");
            Debug.Log("dot targetPos hehe: " + targetPos);
            npc.transform.position = new Vector3(path[pathIndex-1].x, npc.transform.position.y, path[pathIndex-1].z);
            isRotating = false;
        }
    }

    void Move()
    {
        //npc.transform.Translate(speed * Time.deltaTime * npc.transform.forward, Space.World);
        npc.transform.Translate(speed * Time.deltaTime * Vector3.forward, Space.Self);
    }
    /*
    public void Enter()
    {
        CalculatePath();
        pathSpawner.SpawnMarkers(path);

        npcPos = new Vector3(npc.transform.position.x, 0f, npc.transform.position.z);
        targetPos = new Vector3(path[pathIndex].x, 0f, path[pathIndex].z);
        //moveDirection = Vector3.Normalize(targetPos - npcPos);

        targetRotation = RotateTowardsNextPoint(npcPos, targetPos);
        npc.transform.rotation = targetRotation;
        isRotating = true;
    }
    public void Update()
    {
        if (path == null || path.Count == 0 || pathIndex >= path.Count) return;
        

        if (isRotating == true)
        {
            Debug.Log("dot moveDirection: " + moveDirection);
        }

        if (isRotating == true)
        {
            //npc.transform.forward = moveDirection;
            //Quaternion rotationDirection = Quaternion.LookRotation(moveDirection, Vector3.up);
            npc.transform.rotation = Quaternion.RotateTowards(npc.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(npc.transform.rotation, targetRotation) < rotationThreshold)
            {
                npc.transform.rotation = targetRotation;
                targetRotation = Quaternion.identity;
                Debug.Log("dot Zarotirano");
                isRotating = false;
            }

            return;
        }
        

        //Debug.Log("moveDirection: " + moveDirection);

        npcPos = new Vector3(npc.transform.position.x, 0f, npc.transform.position.z);
        if (Vector3.Distance(npcPos, targetPos) < 0.1f)
        {
            Debug.Log("dot Sm blizi:" + targetPos);

            pathIndex++;

            targetPos = new Vector3(path[pathIndex].x, 0f, path[pathIndex].z);
            npcPos = new Vector3(npcPos.x, 0f, npcPos.z);
            moveDirection = Vector3.Normalize(targetPos - npcPos);

            targetRotation = RotateTowardsNextPoint(npcPos, targetPos);
            //isRotating = true;
        }
        Move();

    }

    void Move()
    {
        //npc.transform.Translate(speed * Time.deltaTime * moveDirection, Space.World);
        npc.transform.Translate(speed * Time.deltaTime * npc.transform.forward);
    }
    */
    /*
    Quaternion RotateTowardsNextPoint(Vector3 currentPoint, Vector3 nextPoint)
    {
        if (path == null || path.Count <= pathIndex + 1) return Quaternion.identity; // Need a next point to rotate towards

        Vector3 currentForward = npc.transform.forward;
        currentForward.y = 0f; // Ignore Y-axis for direction
        currentForward.Normalize();

        Vector3 directionToNext = (nextPoint - currentPoint).normalized;
        directionToNext.y = 0f;
        directionToNext.Normalize();
        //Debug.DrawLine(npc.transform.position, currentForward * 2, Color.red, 100f);
        Debug.Log("dot nextPoint: " + nextPoint);
        Debug.Log("dot currentPoint: " + currentPoint);
        Debug.Log("dot directionToNext: " + directionToNext);
        Debug.Log("dot currentForward: " + currentForward);
        Debug.Log("dot directionToNext: " + directionToNext);
        Debug.Log("dot Angle: " + Vector3.Angle(currentForward, directionToNext));

        float dotProduct = Vector3.Dot(currentForward, Vector3.Cross(Vector3.up, directionToNext));
        Quaternion newTargetRotation = Quaternion.identity;

        Debug.Log("dot currentPoint: " + currentPoint);
        Debug.Log("dot nextPoint: " + nextPoint);
        Debug.Log("dot product: " + dotProduct);

        if (dotProduct > 0.1f) // Next point is to the left
        {
            newTargetRotation = npc.transform.rotation * Quaternion.Euler(0f, -90f, 0f);
            isRotating = true;
        }
        else if (dotProduct < -0.1f) // Next point is to the right
        {
            newTargetRotation = npc.transform.rotation * Quaternion.Euler(0f, 90f, 0f);
            isRotating = true;
        }

        return newTargetRotation;
    }
    */

    Quaternion RotateTowardsNextPoint(Vector3 currentPoint, Vector3 nextPoint)
    {
        if (path == null || path.Count <= pathIndex + 1) return Quaternion.identity; // Need a next point to rotate towards

        Vector3 currentRight = npc.transform.right;
        currentRight.y = 0f; // Ignore Y-axis for direction
        currentRight.Normalize();

        Vector3 directionToNext = (nextPoint - currentPoint).normalized;
        directionToNext.y = 0f;
        directionToNext.Normalize();
        //Debug.DrawLine(npc.transform.position, currentForward * 2, Color.red, 100f);
        Debug.Log("dot nextPoint: " + nextPoint);
        Debug.Log("dot currentPoint: " + currentPoint);
        Debug.Log("dot directionToNext: " + directionToNext);
        Debug.Log("dot currentForward: " + currentRight);
        Debug.Log("dot directionToNext: " + directionToNext);
        Debug.Log("dot Angle: " + Vector3.Angle(currentRight, directionToNext));

        float dotProduct = Vector3.Dot(currentRight, directionToNext);
        Quaternion newTargetRotation = npc.transform.rotation;

        Debug.Log("dot currentPoint: " + currentPoint);
        Debug.Log("dot nextPoint: " + nextPoint);
        Debug.Log("dot product: " + dotProduct);

        if (dotProduct > 0.17f) // Next point is to the right
        {
            newTargetRotation = npc.transform.rotation * Quaternion.Euler(0f, 90f, 0f);
            isRotating = true;
        }
        else if (dotProduct < -0.17f) // Next point is to the left
        {
            newTargetRotation = npc.transform.rotation * Quaternion.Euler(0f, -90f, 0f);
            isRotating = true;
        }

        return newTargetRotation;
    }

    void CalculatePath()
    {
        path = pathfinder.FindPath(npc.NextBlock, player.GetNextBlock());
        pathIndex = 1;
    }

    public void Exit()
    {

    }
}
