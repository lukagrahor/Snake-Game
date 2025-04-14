using UnityEngine;

public class ChaseEnemy : Enemy
{
    [SerializeField] AI ai;

    public override void Setup(int col, int row, int gridSize)
    {
        Debug.Log("Niè");
    }

    public void SetupAI(Snake player)
    {
        ai.SetPlayer(player);
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
}
