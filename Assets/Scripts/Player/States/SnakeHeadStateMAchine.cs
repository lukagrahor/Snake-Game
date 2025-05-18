using System;

//[Serializable]
public class SnakeHeadStateMachine : FiniteStateMachine
{
    public SnakeNormalState NormalState;
    public SnakeBitingState BitingState;

    protected Fly npc;
    protected SnakeHead player;
    protected ArenaGrid grid;

    public SnakeHeadStateMachine(SnakeHead player, ArenaGrid grid)
    {
        this.NormalState = new SnakeNormalState(player, grid, this);
        this.BitingState = new SnakeBitingState(player, grid, this);

        this.npc = npc;
        this.player = player;
        this.grid = grid;
    }
    
    public override void Intialize()
    {
        CurrentState = this.NormalState;
        CurrentState.Enter();
    }
}
