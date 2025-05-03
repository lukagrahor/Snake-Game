using UnityEngine;

public class Wasp : Enemy, IFrontTriggerHandler
{
    [SerializeField] WaspAI ai;
    ArenaGrid grid;
    public GridObject NextBlock { get; set; } // je null takoj ob spawnu --> popravi
    public PathSpawner PathSpawner { get; set; }
    public GridObject StartBlock { get; set; }

    public void HandleFrontTrigger()
    {
        GetHit();
    }

    public override void Setup(int col, int row, int gridSize)
    {
        StartBlock = grid.GetGridObjects()[col, row];
        NextBlock = StartBlock;
    }

    public void SetupAI(Snake player, ArenaGrid grid)
    {
        ai.SetPlayer(player);
        ai.SetGrid(grid);
        this.grid = grid;
    }

    protected override void GetHit()
    {
        Destroy(gameObject);
    }

    public void Turn()
    {
        transform.Rotate(0f, 180f, 0f);
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
