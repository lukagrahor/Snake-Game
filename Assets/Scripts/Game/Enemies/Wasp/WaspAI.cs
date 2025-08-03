using UnityEngine;

public class WaspAI : AI
{
    [SerializeField] Wasp npc;
    [SerializeField] PathSpawner pathSpawner;
    public LayerMask LayersToHit { get; set; }
    public WaspStateMachine waspStateMachine { get => (WaspStateMachine)stateMachine; set => stateMachine = value; }
    
    void Start()
    {
        if (player == null)
        {
            return;
        }
        stateMachine = new WaspStateMachine(npc, player, grid, pathSpawner, LayersToHit);
        stateMachine.Intialize();
        waspStateMachine = (WaspStateMachine)stateMachine;
    }
}
