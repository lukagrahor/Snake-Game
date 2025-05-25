using UnityEngine;

public class Fly : Enemy
{
    bool isRotating = false;
    public FlyAI ai;
    public GridObject NextBlock {get; set;}
    public PathSpawner PathSpawner {get; set;}
    public bool IsRotating { get => isRotating; set => isRotating = value; }

    public override void Setup(int col, int row, int gridSize)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IFlytrigger>()?.HandleFlyTrigger(this);
    }

    public void SetupAI(Snake player, ArenaGrid grid)
    {
        ai.SetPlayer(player);
        ai.SetGrid(grid);
    }

    protected override void GetHit()
    {
        Destroy(gameObject);
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
