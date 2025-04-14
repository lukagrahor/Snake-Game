using UnityEngine;

public class AIEnemySpawner : BaseEnemySpawner<ChaseEnemy>
{

    [SerializeField] Snake player;
    protected override void SetupEnemy(ChaseEnemy enemy, GridObject selectedBlock)
    {
        enemy.Setup(selectedBlock.Col, selectedBlock.Row, grid.GetSize());
        enemy.SetupAI(player);
    }
}
