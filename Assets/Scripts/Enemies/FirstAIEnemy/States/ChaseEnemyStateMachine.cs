using System;

[Serializable]
public class ChaseEnemyStateMachine : FiniteStateMachine
{
    public IdleState idleState;
    public PursueState pursueState;

    protected ChaseEnemy npc;
    protected SnakeHead player;
    protected ArenaGrid grid;

    public ChaseEnemyStateMachine(ChaseEnemy npc, SnakeHead player, ArenaGrid grid, PathSpawner pathSpawner)
    {
        this.idleState = new IdleState(npc, player, this);
        this.pursueState = new PursueState(npc, player, this, grid, pathSpawner);

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
