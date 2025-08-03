using UnityEngine;

public class WaspStateMachine : FiniteStateMachine
{
    public WaspPatrolState PatrolState { get; set; }
    public WaspChargeState ChargeState { get; set; }
    Wasp npc;
    SnakeHead player;
    ArenaGrid grid;
    PathSpawner pathSpawner;
    LayerMask layersToHit;

    public WaspStateMachine(Wasp npc, SnakeHead player, ArenaGrid grid, PathSpawner pathSpawner, LayerMask layersToHit)
    {
        this.npc = npc;
        this.player = player;
        this.grid = grid;
        this.pathSpawner = pathSpawner;
        this.layersToHit = layersToHit;
        PatrolState = new WaspPatrolState(npc, player, this, grid, layersToHit);
        ChargeState = new WaspChargeState(npc, player, this, grid);
    }

    public override void Intialize()
    {
        CurrentState = this.PatrolState;
        CurrentState.Enter();
    }
}
