using UnityEngine;

public class DogIdleState : IState
{
    DogStateMachine stateMachine;
    public DogIdleState(DogStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    public void Enter()
    {
        Debug.Log("Entered the idle state");
    }

    public void Update()
    {
        Debug.Log("Dog idling");
    }

    public void Exit()
    {
        Debug.Log("Dog stopping idle");
    }
}