using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    [SerializeField] ChaseEnemy npc;
    [SerializeField] PathSpawner pathSpawner;
    StateMachine stateMachine;
    SnakeHead player;
    ArenaGrid grid;
    void Start()
    {
        Debug.Log("player");
        Debug.Log(player);
        if (player == null)
        {
            Debug.Log("Ne najdem glave!");
            return;
        }
        Debug.Log("naštimej state machine");
        pathSpawner.transform.parent = null;
        stateMachine = new StateMachine(npc, player, grid, pathSpawner);
        stateMachine.Intialize(stateMachine.idleState);
    }

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
