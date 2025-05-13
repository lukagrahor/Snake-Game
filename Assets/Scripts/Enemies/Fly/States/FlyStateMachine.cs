using System;

[Serializable]
public class FlyStateMachine : FiniteStateMachine
{
    public FlyIdleState idleState;
    public FlyPursueState pursueState;

    protected Fly npc;
    protected SnakeHead player;
    protected ArenaGrid grid;

    public FlyStateMachine(Fly npc, SnakeHead player, ArenaGrid grid, PathSpawner pathSpawner)
    {
        this.idleState = new FlyIdleState(npc, player, this, grid);
        this.pursueState = new FlyPursueState(npc, player, this, grid, pathSpawner);

        this.npc = npc;
        this.player = player;
        this.grid = grid;
    }

    public override void Intialize()
    {
        CurrentState = this.idleState;
        CurrentState.Enter();
    }
}
