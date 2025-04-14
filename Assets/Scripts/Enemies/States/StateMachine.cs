using UnityEngine;
using UnityEngine.AI;
using System;

[Serializable]
public class StateMachine
{
    public IState CurrentState { get; private set; }
    
    public IdleState idleState;
    public PursueState pursueState;

    protected GameObject npc;
    protected Transform player;

    public StateMachine(GameObject npc, Transform player)
    {
        this.idleState = new IdleState(npc, player, this);
        this.pursueState = new PursueState(npc, player, this);

        this.npc = npc;
        this.player = player;
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
