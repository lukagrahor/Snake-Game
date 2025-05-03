using UnityEngine;

public class WaspPatrolState : IState
{
    Wasp npc;
    SnakeHead player;
    protected WaspStateMachine stateMachine;
    ArenaGrid grid;

    float moveSpeed = 1.5f;
    enum Directions
    {
        Up = 0,
        Right = 90,
        Down = 180,
        Left = 270
    }
    public WaspPatrolState(Wasp npc, SnakeHead player, WaspStateMachine stateMachine, ArenaGrid grid)
    {
        this.npc = npc;
        this.player = player;
        this.stateMachine = stateMachine;
        this.grid = grid;
    }

    public void Enter()
    {

    }
    public void Update()
    {
        Move();
    }
    public void Exit()
    {

    }

    void Setup()
    {
        //Debug.Log($"col: {col}, row: {row}");
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

    void Move()
    {
        npc.transform.Translate(moveSpeed * Time.deltaTime * Vector3.forward);
    }
}