using UnityEngine;

public class SnakeBitingState : IState
{
    SnakeHead player;
    ArenaGrid grid;
    PathSpawner pathSpawner;
    public SnakeBitingState(SnakeHead player, ArenaGrid grid, SnakeHeadStateMachine stateMachine)
    {
        this.player = player;
        this.grid = grid;
    }

    public void Enter()
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        throw new System.NotImplementedException();
    }
}
