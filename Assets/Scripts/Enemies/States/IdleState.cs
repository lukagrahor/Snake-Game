using UnityEngine;
using UnityEngine.AI;

public class IdleState : IState
{
    protected GameObject npc;
    protected SnakeHead player;
    protected StateMachine stateMachine;
    CountDown timer;
    float waitTime = 3.0f;
    public IdleState(GameObject npc, SnakeHead player, StateMachine stateMachine)
    {
        this.npc = npc;
        this.player = player;
        this.stateMachine = stateMachine;
        timer = new CountDown(waitTime);
        timer.TimeRanOut += StopWaiting;
    }
    
    void StopWaiting()
    {
        stateMachine.TransitionTo(stateMachine.pursueState);
    }

    public void Enter()
    {
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
