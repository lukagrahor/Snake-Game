using System;
using UnityEngine;

//[Serializable]
public class SnakeHeadStateMachine : FiniteStateMachine
{
    public SnakeNormalState NormalState;
    public SnakeBitingState BitingState;

    protected SnakeHead player;

    public SnakeHeadStateMachine(SnakeHead player, LayerMask layersToHit)
    {
        this.NormalState = new SnakeNormalState(player, this);
        this.BitingState = new SnakeBitingState(player, layersToHit, this);

        this.player = player;
    }
    
    public override void Intialize()
    {
        CurrentState = this.NormalState;
        CurrentState.Enter();
    }
}
