using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    StateMachine stateMachine;
    Transform player;

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
        stateMachine = new StateMachine(this.gameObject, player);
        stateMachine.Intialize(stateMachine.idleState);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public void SetPlayer(Snake player)
    {
        this.player = player.transform;
    }
}
