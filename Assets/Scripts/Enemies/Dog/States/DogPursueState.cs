using UnityEngine;

public class DogPursueState : IState
{
    Dog npc;
    SnakeHead player;
    DogStateMachine stateMachine;
    ArenaGrid grid;

    float speed = 1.3f;
    float rotationSpeed = 370f;
    public DogPursueState(Dog npc, SnakeHead player, DogStateMachine stateMachine, ArenaGrid grid)
    {
        this.npc = npc;
        this.player = player;
        this.stateMachine = stateMachine;
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
