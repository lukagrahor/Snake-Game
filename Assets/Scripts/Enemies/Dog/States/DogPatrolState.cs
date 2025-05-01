using System;
using UnityEngine;

public class DogPatrolState : IState
{
    float primaryDirection;
    float secondaryDirection;
    enum Directions
    {
        Up = 0,
        Down = 180,
        Right = 90,
        Left = 270
    }
    DogStateMachine stateMachine;
    public DogPatrolState(DogStateMachine stateMachine)
    {
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

        if (randomDirection == Directions.Up || randomDirection == Directions.Down)
        {
            index = UnityEngine.Random.Range(2, DirectionsValues.Length);
        }
        else if (randomDirection == Directions.Left || randomDirection == Directions.Right)
        {
            index = UnityEngine.Random.Range(0, 2);
        }
        randomDirection = (Directions)DirectionsValues.GetValue(index);
        secondaryDirection = (float)randomDirection;
        Debug.Log("primaryDirection " + primaryDirection);
        Debug.Log("secondaryDirection " + secondaryDirection);
    }

    public void Update()
    {

    }

    public void Exit()
    {

    }
}