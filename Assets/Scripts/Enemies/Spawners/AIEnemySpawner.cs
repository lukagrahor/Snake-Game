using UnityEngine;

public class AIEnemySpawner : BaseEnemySpawner
{
    protected override void SetupEnemy(Enemy enemy, GridObject selectedBlock)
    {
        Fly fly = (Fly)enemy;
        fly.Setup(selectedBlock.Col, selectedBlock.Row, grid.GetSize());
        fly.SetupAI(snake, grid);
    }
}
