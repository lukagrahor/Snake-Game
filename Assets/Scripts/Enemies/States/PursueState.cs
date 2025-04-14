using UnityEngine;
using UnityEngine.AI;

public class PursueState : IState
{
    protected GameObject npc;
    protected Transform player;
    protected StateMachine stateMachine;
    public PursueState(GameObject npc, Transform player, StateMachine stateMachine)
    {
        this.npc = npc;
        this.player = player;
        this.stateMachine = stateMachine;
    }
    public void Enter()
    {
        Debug.Log("Pursue");
    }
    public void Update()
    {
        // uporabi pot od A* algoritma
    }
    public void Exit()
    {

    }
}
