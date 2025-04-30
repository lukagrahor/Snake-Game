using UnityEngine;

public class DogStateMachine : FiniteStateMachine
{
    DogIdleState IdleState { get; set; }
    public DogStateMachine ()
    {
        this.IdleState = IdleState = new DogIdleState(this);
    }

    public override void Intialize()
    {
        CurrentState = this.IdleState;
        CurrentState.Enter();
    }
}
