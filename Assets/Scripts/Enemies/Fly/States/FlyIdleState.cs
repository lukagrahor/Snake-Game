using UnityEngine;
using UnityEngine.AI;

public class FlyIdleState : IState
{
    protected Fly npc;
    protected SnakeHead player;
    protected FlyStateMachine stateMachine;
    CountDown timer;
    public float WaitTime { get; set; }
    public FlyIdleState(Fly npc, SnakeHead player, FlyStateMachine stateMachine)
    {
        this.npc = npc;
        this.player = player;
        this.stateMachine = stateMachine;
        WaitTime = 1f;
    }
    
    void StopWaiting()
    {
        stateMachine.TransitionTo(stateMachine.pursueState);
    }

    public void Enter()
    {
        Debug.Log("Player Idle");
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
