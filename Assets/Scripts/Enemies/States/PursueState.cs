using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

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
    private float rotationSpeed = 720f;

    Vector3 targetPos = Vector3.zero;
    Vector3 npcPos = Vector3.zero;
    Vector3 moveDirection = Vector3.zero;

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
        //Debug.Log("Pursue");
        CalculatePath();
        //Debug.Log("pathSpawner: " + pathSpawner);
        pathSpawner.SpawnMarkers(path);
        // targetPos je 0 na zaèetki ---> narobe
        Debug.Log("Sm path count " + path.Count);
        Debug.Log("Sm path[0]: " + path[pathIndex]);
        npcPos = new Vector3(npc.transform.position.x, 0f, npc.transform.position.z);
        targetPos = new Vector3(path[pathIndex].x, 0f, path[pathIndex].z);
        moveDirection = Vector3.Normalize(targetPos - npcPos);
        Debug.Log("Sm targetPos: " + targetPos);
        Debug.Log("Sm npcPos: " + npcPos);
        Debug.DrawLine(npcPos, npcPos + moveDirection * 5f, Color.red, 1000f, false);
        Debug.Log("Sm moveDirection: " + moveDirection);
        //npc.transform.forward = moveDirection;
    }
    public void Update()
    {
        if (path == null || path.Count == 0 || pathIndex >= path.Count) return;

        //Debug.Log("moveDirection: " + moveDirection);
        float speed = 1.5f;
        npc.transform.Translate(speed * Time.deltaTime * moveDirection, Space.World);
        if (moveDirection != Vector3.zero)
        {
            //npc.transform.forward = moveDirection;
            Quaternion rotationDirection = Quaternion.LookRotation(moveDirection, Vector3.up);
            npc.transform.rotation = Quaternion.RotateTowards(npc.transform.rotation, rotationDirection, rotationSpeed * Time.deltaTime);
        }
        npcPos = new Vector3(npc.transform.position.x, 0f, npc.transform.position.z);
        if (Vector3.Distance(npcPos, targetPos) < 0.1f)
        {
            Debug.Log("Sm blizi");
            Debug.Log(moveDirection);
            targetPos = new Vector3(path[pathIndex].x, 0f, path[pathIndex].z);
            npcPos = new Vector3(npc.transform.position.x, 0f, npc.transform.position.z);
            moveDirection = Vector3.Normalize(targetPos - npcPos);
            //Debug.Log("pathIndex: " + pathIndex);
            // preveè laggy
            // npc.transform.position = new Vector3 (targetPos.x, npc.transform.position.y, targetPos.z);
            pathIndex++;
        }
    }

    void CalculatePath()
    {
        path = pathfinder.FindPath(npc.NextBlock, player.GetNextBlock());
        pathIndex = 0;
    }

    public void Exit()
    {

    }
}
