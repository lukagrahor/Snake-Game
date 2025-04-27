using UnityEngine;
using UnityEngine.AI;

public class IdleState : IState
{
    protected ChaseEnemy npc;
    protected SnakeHead player;
    protected StateMachine stateMachine;
    CountDown timer;
    public float WaitTime { get; set; }
    public IdleState(ChaseEnemy npc, SnakeHead player, StateMachine stateMachine)
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
