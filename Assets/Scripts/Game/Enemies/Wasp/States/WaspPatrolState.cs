using UnityEngine;
public class WaspPatrolState : IState
{
    Wasp npc;
    SnakeHead player;
    protected WaspStateMachine stateMachine;
    ArenaGrid grid;
    LayerMask layersToHit;
    float rayMaxDistance = 50f;
    Color baseColor;
    Color previousColor;

    float moveSpeed = 1.5f;
    enum Directions
    {
        Up = 0,
        Right = 90,
        Down = 180,
        Left = 270
    }
    public WaspPatrolState(Wasp npc, SnakeHead player, WaspStateMachine stateMachine, ArenaGrid grid, LayerMask layersToHit)
    {
        this.npc = npc;
        this.player = player;
        this.stateMachine = stateMachine;
        this.grid = grid;
        this.layersToHit = layersToHit;
    }

    public void Enter()
    {
        previousColor = npc.WaspRenderer.sharedMaterial.color;
        baseColor = npc.WaspColor;
    }
    public void Update()
    {
        if (npc.WaspRenderer.sharedMaterial.color != baseColor)
        {
            npc.WaspRenderer.sharedMaterial.color = Color.Lerp(previousColor, baseColor, Mathf.PingPong(Time.time, 1.5f));
        }
        CheckForPlayer();
        Move();
    }
    public void Exit()
    {

    }
    /*
    void Setup()
    {
        GridObject startBlock = npc.StartBlock;
        int gridSize = grid.GetSize();
        int col = startBlock.Col;
        int row = startBlock.Row;
        if (row == 0 && col == 0)
        {
            int random = Random.Range(1, 3);
            if (random == 1)
            {
                npc.transform.Rotate(0f, (float)Directions.Right, 0f);
            }
        }
        else if (row == (gridSize - 1) && col == (gridSize - 1))
        {
            int random = Random.Range(1, 3);
            if (random == 1)
            {
                npc.transform.Rotate(0f, (float)Directions.Down, 0f);
            }
            else
            {
                npc.transform.Rotate(0f, (float)Directions.Left, 0f);
            }
        }
        else if (col == 0)
        {
            npc.transform.Rotate(0f, (float)Directions.Right, 0f);
        }
        else if (col == (gridSize - 1))
        {
            npc.transform.Rotate(0f, (float)Directions.Left, 0f);
        }
        else if (row == (gridSize - 1))
        {
            npc.transform.Rotate(0f, (float)Directions.Down, 0f);
        }
    }
    */
    void CheckForPlayer()
    {
        Ray ray = new Ray(npc.transform.position, npc.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, rayMaxDistance, layersToHit))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("SnakeHead") || hit.collider.gameObject.layer == LayerMask.NameToLayer("SnakeBody"))
            {
                stateMachine.TransitionTo(stateMachine.ChargeState);
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy") && hit.distance < 0.3f)
            {
                npc.Turn();
            }
        }
    }

    void Move()
    {
        npc.transform.Translate(moveSpeed * Time.deltaTime * Vector3.forward);
    }
}