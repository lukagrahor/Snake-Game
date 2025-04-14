using UnityEngine;
using UnityEngine.AI;

public class IdleState : IState
{
    protected GameObject npc;
    protected Transform player;
    protected StateMachine stateMachine;
    public IdleState(GameObject npc, Transform player, StateMachine stateMachine)
    {
        this.npc = npc;
        this.player = player;
        this.stateMachine = stateMachine;
    }
    public void Enter()
    {
        
    }
    public void Update()
    {
        
    }
    public void Exit()
    {
       
    }
}
