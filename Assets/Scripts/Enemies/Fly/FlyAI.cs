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
        Debug.Log("tle sm 1");
        stateMachine = new FlyStateMachine(npc, player, grid, pathSpawner);
        Debug.Log("tle sm 2");
        stateMachine.Intialize();
    }
}
