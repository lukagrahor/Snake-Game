using UnityEngine;

public class WaspStateMachine : FiniteStateMachine
{
    public WaspPatrolState PatrolState { get; set; }
    Wasp npc;
    SnakeHead player;
    ArenaGrid grid;
    PathSpawner pathSpawner;

    public WaspStateMachine(Wasp npc, SnakeHead player, ArenaGrid grid, PathSpawner pathSpawner)
    {
        this.npc = npc;
        this.player = player;
        this.grid = grid;
        this.pathSpawner = pathSpawner;
        PatrolState = new WaspPatrolState(npc, player, this, grid);
    }

    public override void Intialize()
    {
        CurrentState = this.PatrolState;
        CurrentState.Enter();
    }
}
