using UnityEngine;

public class Wasp : Enemy
{
    [SerializeField] WaspAI ai;
    [SerializeField] LayerMask layersToHit;
    ArenaGrid grid;
    public GridObject NextBlock { get; set; } // je null takoj ob spawnu --> popravi
    public PathSpawner PathSpawner { get; set; }
    public GridObject StartBlock { get; set; }
    public WaspAI Ai { get => ai; set => ai = value; }
    public Renderer WaspRenderer { get; set; }
    public Color WaspColor { get; set; }

    public override void Setup(int col, int row, int gridSize)
    {
        StartBlock = grid.GetGridObjects()[col, row];
        NextBlock = StartBlock;
    }

    public override void SetupAI(Snake player, ArenaGrid grid)
    {
        ai.SetPlayer(player);
        ai.SetGrid(grid);
        this.grid = grid;
        ai.LayersToHit = layersToHit;
    }

    protected override void GetHit()
    {
        Destroy(gameObject);
        enemyDied?.Invoke();
    }

    public void Turn()
    {
        transform.Rotate(0f, 180f, 0f);
    }

    void Start()
    {
        WaspRenderer = GetComponent<Renderer>();
        WaspColor = WaspRenderer.material.color;
    }

    void Update()
    {

    }
}
