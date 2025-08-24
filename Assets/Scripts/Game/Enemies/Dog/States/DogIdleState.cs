using UnityEngine;

public class DogIdleState : IState
{
    protected DogStateMachine stateMachine;
    CountDown timer;
    public float WaitTime { get; set; }
    public DogIdleState(DogStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        WaitTime = 1.5f;
    }

    void StopWaiting()
    {
        stateMachine.TransitionTo(stateMachine.PatrolState);
    }

    public void Enter()
    {
        timer = new CountDown(WaitTime);
        timer.TimeRanOut += StopWaiting;
        timer.Start();
    }
    public void Update()
    {
        timer.Update();
    }
    public void Exit()
    {

    }
}