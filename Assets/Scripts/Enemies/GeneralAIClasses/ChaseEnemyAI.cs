using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;

public class ChaseEnemyAI : AI
{
    [SerializeField] ChaseEnemy npc;
    [SerializeField] PathSpawner pathSpawner;
    void Start()
    {
        //Debug.Log("player");
        //Debug.Log(player);
        if (player == null)
        {
            Debug.Log("Ne najdem glave!");
            return;
        }
        //Debug.Log("naštimej state machine");
        pathSpawner.transform.parent = null;
        stateMachine = new ChaseEnemyStateMachine(npc, player, grid, pathSpawner);
        stateMachine.Intialize();
    }
}
