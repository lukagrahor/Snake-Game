using UnityEngine;

public class DogStateMachine : FiniteStateMachine
{
    public DogIdleState IdleState { get; set; }
    public DogPatrolState PatrolState { get; set; }
    Dog npc;
    SnakeHead player;
    ArenaGrid grid;
    PathSpawner pathSpawner;

    public DogStateMachine (Dog npc, SnakeHead player, ArenaGrid grid, PathSpawner pathSpawner)
    {
        this.npc = npc;
        this.player = player;
        this.grid = grid;
        this.pathSpawner = pathSpawner;
        IdleState = new DogIdleState(this);
        PatrolState = new DogPatrolState(npc, player, this, grid);
    }

    public override void Intialize()
    {
        CurrentState = this.IdleState;
        CurrentState.Enter();
    }
}
