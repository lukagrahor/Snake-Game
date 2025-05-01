using UnityEngine;

public class Dog : Enemy
{
    [SerializeField] DogAI ai;
    public GridObject NextBlock { get; set; }
    public PathSpawner PathSpawner { get; set; }

    public override void Setup(int col, int row, int gridSize)
    {
        Debug.Log("Niè");
    }
    /* ne tu pozabt !!!
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IChaseEnemytrigger>()?.HandleChaseEnemyTrigger(this);
    }
    */

    public void SetupAI(Snake player, ArenaGrid grid)
    {
        ai.SetPlayer(player);
        ai.SetGrid(grid);
    }

    protected override void GetHit()
    {
        Debug.Log("Niè");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        IDogTriggerHandler enteredObject = other.GetComponent<IDogTriggerHandler>();
        if (ai.DogStateMachine != null) enteredObject?.HandleTrigger(ai.DogStateMachine.PatrolState);
    }
}
