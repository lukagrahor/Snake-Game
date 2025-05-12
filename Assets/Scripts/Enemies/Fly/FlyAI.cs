using UnityEngine;

public class FlyAI : AI
{
    [SerializeField] Fly npc;
    [SerializeField] PathSpawner pathSpawner;
    void Start()
    {
        //Debug.Log("player");
        if (player == null)
        {
            Debug.Log("Ne najdem glave!");
            return;
        }
        //Debug.Log("naštimej state machine");
        pathSpawner.transform.parent = null;
        stateMachine = new FlyStateMachine(npc, player, grid, pathSpawner);
        stateMachine.Intialize();
    }
}
