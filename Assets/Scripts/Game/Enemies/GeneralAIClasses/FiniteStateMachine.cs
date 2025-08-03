using UnityEngine;

public abstract class FiniteStateMachine
{
    public IState CurrentState { get; set; }

    public abstract void Intialize();

    public void TransitionTo(IState nextState)
    {
        CurrentState.Exit();
        CurrentState = nextState;
        nextState.Enter();
    }

    public void Update()
    {
        CurrentState?.Update();
    }
}
