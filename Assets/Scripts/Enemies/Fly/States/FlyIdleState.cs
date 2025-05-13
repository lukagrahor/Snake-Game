using UnityEngine;
using UnityEngine.AI;

public class FlyIdleState : IState
{
    protected Fly npc;
    protected SnakeHead player;
    protected FlyStateMachine stateMachine;
    protected ArenaGrid grid;
    CountDown timer;
    float waitTime = 2f;
    bool timerRunOut = false;
    //public float WaitTime { get; set; }
    public FlyIdleState(Fly npc, SnakeHead player, FlyStateMachine stateMachine, ArenaGrid grid)
    {
        this.npc = npc;
        this.player = player;
        this.stateMachine = stateMachine;
        this.grid = grid;
        //WaitTime = 1f;
    }
    
    void StopWaiting()
    {
        stateMachine.TransitionTo(stateMachine.pursueState);
    }

    public void Enter()
    {
        Debug.Log("Fly Idle");
        timerRunOut = false;
        timer = new CountDown(waitTime);
        timer.TimeRanOut += CheckForFood;
        timer.Start();
    }
    public void Update()
    {
        timer.Update();
        if (timerRunOut) CheckForFood();
    }
    public void Exit()
    {
       
    }
    void CheckForFood()
    {
        if (grid.ObjectsWithFood.Count > 0)
        {
            stateMachine.TransitionTo(stateMachine.pursueState);
        }
        timerRunOut = true;
    }
}
