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
    FindPathAStar pathfinder;
    List<Vector3> path;
    int pathIndex;
    private float repathCooldown = 0.5f;
    private float repathTimer = 0f;

    public PursueState(ChaseEnemy npc, SnakeHead player, StateMachine stateMachine, ArenaGrid grid)
    {
        this.npc = npc;
        this.player = player;
        this.stateMachine = stateMachine;
        pathfinder = new FindPathAStar(grid);
    }
    public void Enter()
    {
        Debug.Log("Pursue");
        CalculatePath();
        Debug.Log("npc.transform.forward1: " + npc.transform.forward);
        Vector3 targetPos = new Vector3 (path[1].x, 0f, path[1].z);
        Vector3 npcPos = new Vector3(npc.transform.position.x, 0f, npc.transform.position.z);
        Vector3 dir = (targetPos - npcPos).normalized;
        Debug.Log("dir1: " + dir);
        Debug.Log("dir1 npc.transform.right: " + npc.transform.right);
        Debug.Log("dir1 dot product: " + Vector3.Dot(npc.transform.right, dir));
    }
    public void Update()
    {
        if (path == null || path.Count == 0 || pathIndex >= path.Count) return;
        Vector3 targetPos = path[pathIndex];
        float speed = 1f;

        Vector3 dir = (targetPos - npc.transform.position).normalized;
        Debug.Log("targetPos: " + targetPos);
        Debug.Log("npc.transform.forward: " + npc.transform.forward);
        Debug.Log("dir: " + dir);
        npc.transform.Translate(speed * Time.deltaTime * Vector3.forward);

        if (Vector3.Distance(new Vector3(npc.transform.position.x, 0f, npc.transform.position.z), new Vector3(targetPos.x, 0f, targetPos.z)) < 0.1f)
        {
            // rotiraj v pravo smer

            float rotationAngle = Vector3.Angle(npc.transform.forward, dir);
            Debug.Log("Smer: " + rotationAngle);
            npc.transform.Rotate(new Vector3(0f, rotationAngle, 0f));
            npc.transform.position = targetPos;
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
