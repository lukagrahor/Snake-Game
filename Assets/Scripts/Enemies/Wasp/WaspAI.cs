using UnityEngine;

public class WaspAI : AI
{
    [SerializeField] Wasp npc;
    [SerializeField] PathSpawner pathSpawner;
    public WaspStateMachine DogStateMachine { get => (WaspStateMachine)stateMachine; set => stateMachine = value; }
    
    void Start()
    {
        if (player == null)
        {
            Debug.Log("Ne najdem glave!");
            return;
        }
        //Debug.Log("naštimej state machine");
        //pathSpawner.transform.parent = null;
        stateMachine = new WaspStateMachine(npc, player, grid, pathSpawner);
        stateMachine.Intialize();
        DogStateMachine = (WaspStateMachine)stateMachine;
    }
}
