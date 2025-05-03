using UnityEngine;

public class WaspSpawner : BaseEnemySpawner<Wasp>
{
    protected override void SetupEnemy(Wasp enemy, GridObject selectedBlock)
    {
        enemy.SetupAI(snake, grid);
        enemy.Setup(selectedBlock.Col, selectedBlock.Row, grid.GetSize());
    }
}
