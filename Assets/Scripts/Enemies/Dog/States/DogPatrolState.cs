using System;
using UnityEngine;

public class DogPatrolState : IState
{
    float primaryDirection;
    float secondaryDirection;
    float speed = 1.5f;
    float rotationSpeed = 400f;
    bool isRotating = false;
    bool isChangingLane = false;
    float rotationThreshold = 5f;
    Dog npc;
    SnakeHead player;
    DogStateMachine stateMachine;

    enum Directions
    {
        Up = 0,
        Right = 90,
        Down = 180,
        Left = 270
    }

    public DogPatrolState(Dog npc, SnakeHead player, DogStateMachine stateMachine)
    {
        this.npc = npc;
        this.player = player;
        this.stateMachine = stateMachine;
    }
    // doloèi ali pes gre gor dol al levo desno --> random

    public void Enter()
    {
        Debug.Log("Entered the patrol state");
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
        }
        else if (randomDirection == Directions.Left || randomDirection == Directions.Right)
        {
            float[] secondaryDirections = { 0f, 180f };
            index = UnityEngine.Random.Range(0, 2);
            secondaryDirection = secondaryDirections[index];
        }

        Debug.Log("secondaryDirection " + secondaryDirection + " index " + index);
        npc.transform.eulerAngles = new Vector3(0f, primaryDirection, 0f);
    }

    public void Update()
    {
        if (isRotating)
        {
            /*if (isChangingLane)
            {

            }*/
            Debug.Log("Rotation time!");
            Quaternion targetRotation = Quaternion.Euler(0f, secondaryDirection, 0f);
            npc.transform.rotation = Quaternion.RotateTowards(npc.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            if (Quaternion.Angle(npc.transform.rotation, targetRotation) < rotationThreshold)
            {
                npc.transform.rotation = targetRotation;
                targetRotation = Quaternion.identity;
                isRotating = false;
            }
            //npc.transform.Rotate(Vector3.up, secondaryDirection);
            //isRotating = false;
            return;
        }
        Move();
    }

    public void ChangeLane()
    {
        Debug.Log("Menjej pas");
        isRotating = true;
        //isChangingLane = true;
    }

    void Move()
    {
        npc.transform.Translate(speed * Time.deltaTime * Vector3.forward);
    }

    public void Exit()
    {

    }
}