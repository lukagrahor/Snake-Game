using UnityEngine;

public class Dog : Enemy, IPathTrigger
{
    [SerializeField] DogAI ai;
    SnakePathMarker firstMarker;
    ArenaGrid grid;
    public GridObject NextBlock { get; set; } // je null takoj ob spawnu --> popravi
    public PathSpawner PathSpawner { get; set; }
    public GridObject StartBlock { get; set; }
    public SnakePathMarker FirstMarker { get => firstMarker; set => firstMarker = value; }

    public override void Setup(int col, int row, int gridSize)
    {
        StartBlock = grid.GetGridObjects()[col, row];
        NextBlock = StartBlock;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDogTriggerHandler enteredObject = other.GetComponent<IDogTriggerHandler>();
        if (ai.DogStateMachine != null && ai.DogStateMachine.CurrentState == ai.DogStateMachine.PatrolState)
            enteredObject?.HandleTrigger(ai.DogStateMachine.CurrentState, this);
    }

    public override void SetupAI(Snake player, ArenaGrid grid)
    {
        ai.SetPlayer(player);
        ai.SetGrid(grid);
        this.grid = grid;
    }

    protected override void GetHit()
    {
        Destroy(gameObject);
        enemyDied?.Invoke();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HandlePathTrigger(SnakePathMarker marker)
    {
        DogStateMachine stateMachine = (DogStateMachine)ai.stateMachine;
        if (stateMachine.CurrentState == stateMachine.PatrolState)
        {
            firstMarker = marker;
            stateMachine.TransitionTo(stateMachine.PursueState);
        }
    }
}
