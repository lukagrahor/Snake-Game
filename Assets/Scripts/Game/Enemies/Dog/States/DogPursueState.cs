using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DogPursueState : IState
{
    Dog npc;
    SnakeHead player;
    DogStateMachine stateMachine;
    ArenaGrid grid;
    SnakePath pathObject;

    float speed = 0.97f;
    float rotationSpeed = 10f;
    bool isRotating = false;
    int index;
    Quaternion nextRotation;
    Quaternion nextTargetRotation;
    float rotationTreshold = 5f;
    public DogPursueState(Dog npc, SnakeHead player, DogStateMachine stateMachine, ArenaGrid grid)
    {
        this.npc = npc;
        this.player = player;
        this.stateMachine = stateMachine;
        this.grid = grid;
    }
    public void Enter()
    {
        Vector3 firstMarkerPosition = npc.FirstMarker.transform.position;
        npc.transform.position = new Vector3(firstMarkerPosition.x, npc.transform.position.y, firstMarkerPosition.z);
        pathObject = player.GetSnake().Path;
        List<SnakePathMarker> path = pathObject.Path;
        index = path.IndexOf(npc.FirstMarker);
        index++;
        if(index >= path.Count || index < 0)
        {
            stateMachine.TransitionTo(stateMachine.PatrolState);
            return;
        }
        SnakePathMarker nextMarker = path[index];
        Vector3 nextMarkerPosition = new Vector3(nextMarker.transform.position.x, npc.transform.position.y, nextMarker.transform.position.z);
        Vector3 directionToNext = (nextMarkerPosition - npc.transform.position).normalized;
        npc.transform.forward = directionToNext;

        PlayerActions.PlayerDeath += TransitionToPatrol;
    }

    public void Exit()
    {
        // centrirej ga na kocko
        if (npc == null) return;
        npc.transform.position = npc.NextBlock.transform.position;
        npc.StartBlock = npc.NextBlock;
        PlayerActions.PlayerDeath -= TransitionToPatrol;
    }

    public void Update()
    {
        CheckForTurn();
        Rotate();
        Move();
    }

    void Move()
    {
        if (isRotating) return;
        npc.transform.Translate(speed * Time.deltaTime * Vector3.forward);
    }

    void CheckForTurn()
    {
        if (isRotating) return;
        List<SnakePathMarker> path = pathObject.Path;
        if (index >= path.Count)
        {
            stateMachine.TransitionTo(stateMachine.PatrolState);
            return;
        }
        SnakePathMarker currentMarker = path[index];
        Vector3 targetPos = new Vector3(currentMarker.transform.position.x, 0f, currentMarker.transform.position.z);
        Vector3 npcPos = new Vector3(npc.transform.position.x, 0f, npc.transform.position.z);

        Vector3 moveDirection = (targetPos - npcPos).normalized;
        float dotProduct = Vector3.Dot(npc.transform.forward, moveDirection);
        float distance = Vector3.Distance(npcPos, targetPos);
        if (distance <= 0.01f || (dotProduct < 0 && distance <= 0.1f))
        {
            index++;
            if (currentMarker.NextRotation == 0f) return;
            nextRotation = npc.transform.rotation * Quaternion.Euler(0f, currentMarker.NextRotation, 0f);
            npc.transform.position = new Vector3(targetPos.x, npc.transform.position.y, targetPos.z);
            isRotating = true;
        }
    }   
    
    void Rotate()
    {
        if (!isRotating) return;
        npc.transform.rotation = Quaternion.Lerp(npc.transform.rotation, nextRotation, rotationSpeed * Time.deltaTime);
        if (Quaternion.Angle(npc.transform.rotation, nextRotation) < rotationTreshold)
        {
            npc.transform.rotation = nextRotation;
            nextRotation = Quaternion.identity;
            isRotating = false;
        }
    }

    void TransitionToPatrol()
    {
        stateMachine.TransitionTo(stateMachine.IdleState);
    }
}
