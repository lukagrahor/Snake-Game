using UnityEngine;

public class SnakeNormalState : IState
{
    SnakeHead player;
    ArenaGrid grid;
    PathSpawner pathSpawner;
    public SnakeNormalState(SnakeHead player, ArenaGrid grid, SnakeHeadStateMachine stateMachine)
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
