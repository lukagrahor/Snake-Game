using System;

//[Serializable]
public class SnakeHeadStateMachine : FiniteStateMachine
{
    public SnakeNormalState NormalState;
    public SnakeBitingState BitingState;

    protected SnakeHead player;

    public SnakeHeadStateMachine(SnakeHead player)
    {
        this.NormalState = new SnakeNormalState(player, this);
        this.BitingState = new SnakeBitingState(player, this);

        this.player = player;
    }
    
    public override void Intialize()
    {
        CurrentState = this.NormalState;
        CurrentState.Enter();
    }
}
