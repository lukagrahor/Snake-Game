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

    float speed = 1f;
    float rotationSpeed = 370f;
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
        Debug.Log("Pursue");
        npc.transform.position = npc.FirstMarker.transform.position;
        pathObject = player.GetSnake().Path;
        List<SnakePathMarker> path = pathObject.Path;
        index = path.IndexOf(npc.FirstMarker);
        Debug.Log("First index " + index);
        Debug.Log("Prvi marker " + npc.FirstMarker);
        index++;
        if(index >= path.Count)
        {
            stateMachine.TransitionTo(stateMachine.PatrolState);
        }
        SnakePathMarker nextMarker = path[index];
        Vector3 directionToNext = (nextMarker.transform.position - npc.transform.position).normalized;
        npc.transform.forward = directionToNext;

        Debug.Log("directionToNext " + directionToNext);
    }

    public void Exit()
    {
        Debug.Log("Konec pursue");
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
        SnakePathMarker currentMarker = path[index];
        Vector3 targetPos = new Vector3(currentMarker.transform.position.x, 0f, currentMarker.transform.position.z);
        Vector3 npcPos = new Vector3(npc.transform.position.x, 0f, npc.transform.position.z);

        Vector3 moveDirection = (targetPos - npcPos).normalized;
        float dotProduct = Vector3.Dot(npc.transform.forward, moveDirection);
        float distance = Vector3.Distance(npcPos, targetPos);
        //Debug.Log("distance:" + distance);
        //Debug.Log("dotProduct:" + dotProduct);
        if (distance <= 0.01f || (dotProduct < 0 && distance <= 0.1f))
        {
            Debug.Log("NextRotation current index " + index);
            index++;
            if (currentMarker.NextRotation == 0f) return;
            Debug.Log("NextRotation " + currentMarker.NextRotation);
            //nextRotation = Quaternion.AngleAxis(currentMarker.NextRotation, Vector3.up);
            nextRotation = npc.transform.rotation * Quaternion.Euler(0f, currentMarker.NextRotation, 0f);
            Debug.Log("NextRotation Angle axis" + currentMarker.NextRotation);
            npc.transform.position = new Vector3(targetPos.x, npc.transform.position.y, targetPos.z);
            isRotating = true;
        }
    }   
    
    void Rotate()
    {
        if (!isRotating) return;
        Debug.Log("O moj bog!");
        //Quaternion.RotateTowards(npc.transform.rotation, Quaternion.Euler(0f, nextRotation, 0f);
        npc.transform.rotation = Quaternion.Lerp(npc.transform.rotation, nextRotation, 3f * Time.deltaTime);
        if (Quaternion.Angle(npc.transform.rotation, nextRotation) < rotationTreshold)
        {
            Debug.Log("NextRotation zadwasti");
            npc.transform.rotation = nextRotation;
            nextRotation = Quaternion.identity;
            isRotating = false;
        }
    }
}
