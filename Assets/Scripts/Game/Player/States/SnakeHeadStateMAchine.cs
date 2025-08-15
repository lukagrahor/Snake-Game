using System;
using UnityEngine;

//[Serializable]
public class SnakeHeadStateMachine : FiniteStateMachine
{
    public SnakeSpawnedState SpawnedState;
    public SnakeNormalState NormalState;
    public SnakePowerState PowerState;

    public SnakeBitingState BitingState;
    public SnakeSpittingState SpittingState;

    protected SnakeHead player;

    public SnakeHeadStateMachine(SnakeHead player, LayerMask layersToHit)
    {
        this.BitingState = new SnakeBitingState(player, layersToHit, this);
        this.SpittingState = new SnakeSpittingState(player, layersToHit, this);

        this.SpawnedState = new SnakeSpawnedState(player, this);
        this.NormalState = new SnakeNormalState(player, this);
        this.PowerState = BitingState;

        this.player = player;
    }
    
    public override void Intialize()
    {
        CurrentState = this.SpawnedState;
        CurrentState.Enter();
    }
}
