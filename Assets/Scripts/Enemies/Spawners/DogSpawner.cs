using UnityEngine;

public class DogSpawner : BaseEnemySpawner
{
    protected override void SetupEnemy(Enemy enemy, GridObject selectedBlock)
    {
        Dog dog = (Dog)enemy;
        dog.SetupAI(snake, grid);
        dog.Setup(selectedBlock.Col, selectedBlock.Row, grid.GetSize());
    }
}
