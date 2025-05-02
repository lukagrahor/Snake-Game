using UnityEngine;

public class Dog : Enemy
{
    [SerializeField] DogAI ai;
    ArenaGrid grid;
    public GridObject NextBlock { get; set; } // je null takoj ob spawnu --> popravi
    public PathSpawner PathSpawner { get; set; }
    public GridObject StartBlock { get; set; }

    public override void Setup(int col, int row, int gridSize)
    {
        StartBlock = grid.GetGridObjects()[col, row];
        NextBlock = StartBlock;
        Debug.Log("Dog startBlock " + StartBlock);
    }

    private void OnTriggerEnter(Collider other)
    {
        IDogTriggerHandler enteredObject = other.GetComponent<IDogTriggerHandler>();
        if (ai.DogStateMachine != null) enteredObject?.HandleTrigger(ai.DogStateMachine.PatrolState, this);
    }

    public void SetupAI(Snake player, ArenaGrid grid)
    {
        ai.SetPlayer(player);
        ai.SetGrid(grid);
        this.grid = grid;
    }

    protected override void GetHit()
    {
        Debug.Log("Ni�");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
