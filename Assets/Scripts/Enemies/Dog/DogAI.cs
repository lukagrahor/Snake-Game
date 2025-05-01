using UnityEngine;

public class DogAI : AI
{
    [SerializeField] Dog npc;
    [SerializeField] PathSpawner pathSpawner;
    public DogStateMachine DogStateMachine { get => (DogStateMachine)stateMachine; set => stateMachine = value; }
    
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
        //pathSpawner.transform.parent = null;
        stateMachine = new DogStateMachine(npc, player, grid, pathSpawner);
        stateMachine.Intialize();
        DogStateMachine = (DogStateMachine)stateMachine;
    }
}
