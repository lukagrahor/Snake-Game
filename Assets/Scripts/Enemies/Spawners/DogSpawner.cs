using UnityEngine;

public class DogSpawner : BaseEnemySpawner<Dog>
{
    protected override void SetupEnemy(Dog enemy, GridObject selectedBlock)
    {
        enemy.Setup(selectedBlock.Col, selectedBlock.Row, grid.GetSize());
        enemy.SetupAI(snake, grid);
    }
}
