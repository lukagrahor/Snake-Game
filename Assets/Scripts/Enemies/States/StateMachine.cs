using UnityEngine;
using UnityEngine.AI;
using System;

[Serializable]
public class StateMachine
{
    public IState CurrentState { get; private set; }
    
    public IdleState idleState;
    public PursueState pursueState;

    protected ChaseEnemy npc;
    protected SnakeHead player;
    protected ArenaGrid grid;

    public StateMachine(ChaseEnemy npc, SnakeHead player, ArenaGrid grid)
    {
        this.idleState = new IdleState(npc, player, this);
        this.pursueState = new PursueState(npc, player, this, grid);

        this.npc = npc;
        this.player = player;
        this.grid = grid;
    }

    public void Intialize(IState startingState)
    {
        CurrentState = startingState;
        startingState.Enter();
    }

    public void TransitionTo(IState nextState)
    {
        CurrentState.Exit();
        CurrentState = nextState;
        nextState.Enter();
    }

    public void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.Update();
        }
    }
}
