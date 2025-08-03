using UnityEngine;

public class WaspChargeState : IState
{
    Wasp npc;
    SnakeHead player;
    protected WaspStateMachine stateMachine;
    ArenaGrid grid;
    float moveSpeed = 2.5f;

    Color baseColor;
    Color chargeStateColor = Color.red;

    public WaspChargeState(Wasp npc, SnakeHead player, WaspStateMachine stateMachine, ArenaGrid grid)
    {
        this.npc = npc;
        this.player = player;
        this.stateMachine = stateMachine;
        this.grid = grid;
    }

    public void Enter()
    {
        baseColor = npc.WaspColor;
        Debug.Log("Zaènem charge");
    }
    public void Update()
    {
        npc.WaspRenderer.material.color = Color.Lerp(baseColor, chargeStateColor, Mathf.PingPong(Time.time, 1.5f));
        Move();
    }
    public void Exit()
    {
        Debug.Log("Konèam charge");
    }

    public void CoolDown()
    {
        stateMachine.TransitionTo(stateMachine.PatrolState);
    }

    void Move()
    {
        npc.transform.Translate(moveSpeed * Time.deltaTime * Vector3.forward);
    }
}
