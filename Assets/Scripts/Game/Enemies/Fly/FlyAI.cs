using UnityEngine;

public class FlyAI : AI
{
    [SerializeField] Fly npc;
    [SerializeField] PathSpawner pathSpawner;
    void Start()
    {
        if (player == null)
        {
            return;
        }
        pathSpawner.transform.parent = null;
        stateMachine = new FlyStateMachine(npc, player, grid, pathSpawner);
        stateMachine.Intialize();
    }
}
