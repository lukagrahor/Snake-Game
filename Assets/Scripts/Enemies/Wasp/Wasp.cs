using UnityEngine;

public class Wasp : Enemy
{
    [SerializeField] WaspAI ai;
    ArenaGrid grid;
    public GridObject NextBlock { get; set; } // je null takoj ob spawnu --> popravi
    public PathSpawner PathSpawner { get; set; }
    public GridObject StartBlock { get; set; }

    public override void Setup(int col, int row, int gridSize)
    {
        StartBlock = grid.GetGridObjects()[col, row];
        NextBlock = StartBlock;
    }

    private void OnTriggerEnter(Collider other)
    {
        IWaspTriggerHandler enteredObject = other.GetComponent<IWaspTriggerHandler>();
        if (ai.DogStateMachine != null) enteredObject?.HandleTrigger(this);
    }

    public void SetupAI(Snake player, ArenaGrid grid)
    {
        ai.SetPlayer(player);
        ai.SetGrid(grid);
        this.grid = grid;
    }

    protected override void GetHit()
    {
        Debug.Log("Niè");
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
