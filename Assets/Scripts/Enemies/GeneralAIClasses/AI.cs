using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;

public abstract class AI : MonoBehaviour
{
    protected FiniteStateMachine stateMachine;
    protected SnakeHead player;
    protected ArenaGrid grid;

    private void Update()
    {
        stateMachine.Update();
    }

    public void SetPlayer(Snake player)
    {
        this.player = player.SnakeHead;
    }

    public void SetGrid(ArenaGrid grid)
    {
        this.grid = grid;
    }
}
