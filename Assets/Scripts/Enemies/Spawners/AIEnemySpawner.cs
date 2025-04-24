using UnityEngine;

public class AIEnemySpawner : BaseEnemySpawner<ChaseEnemy>
{
    protected override void SetupEnemy(ChaseEnemy enemy, GridObject selectedBlock)
    {
        enemy.Setup(selectedBlock.Col, selectedBlock.Row, grid.GetSize());
        //Debug.Log("tle je player");
        //Debug.Log(snake);
        enemy.SetupAI(snake, grid);
    }
}
