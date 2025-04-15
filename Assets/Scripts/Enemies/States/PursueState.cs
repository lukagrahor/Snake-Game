using UnityEngine;
using UnityEngine.AI;

public class PursueState : IState
{
    protected GameObject npc;
    protected SnakeHead player;
    protected StateMachine stateMachine;
    FindPathAStar pathfinding;

    public PursueState(GameObject npc, SnakeHead player, StateMachine stateMachine, ArenaGrid grid)
    {
        this.npc = npc;
        this.player = player;
        this.stateMachine = stateMachine;
        pathfinding = new FindPathAStar(grid);
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
