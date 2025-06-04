using UnityEngine;

public abstract class FiniteStateMachine
{
    public IState CurrentState { get; set; }

    public abstract void Intialize();

    public void TransitionTo(IState nextState)
    {
        if (CurrentState == null)
        {
            Debug.Log("Majstr je nulas");
        } else
        {
            Debug.Log("Majstr je ful kul");
        }
        CurrentState.Exit();
        CurrentState = nextState;
        nextState.Enter();
    }

    public void Update()
    {
        CurrentState?.Update();
    }
}
