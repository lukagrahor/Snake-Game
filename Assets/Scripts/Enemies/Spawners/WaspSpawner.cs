using UnityEngine;

public class WaspSpawner : BaseEnemySpawner
{
    protected override void SetupEnemy(Enemy enemy, GridObject selectedBlock)
    {
        Wasp wasp = (Wasp)enemy;
        wasp.SetupAI(snake, grid);
        wasp.Setup(selectedBlock.Col, selectedBlock.Row, grid.GetSize());
    }
}
