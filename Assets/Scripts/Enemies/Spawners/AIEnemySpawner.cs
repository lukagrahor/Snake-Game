using UnityEngine;

public class AIEnemySpawner : BaseEnemySpawner<Fly>
{
    protected override void SetupEnemy(Fly enemy, GridObject selectedBlock)
    {
        enemy.Setup(selectedBlock.Col, selectedBlock.Row, grid.GetSize());
        //Debug.Log("tle je player");
        //Debug.Log(snake);
        enemy.SetupAI(snake, grid);
    }
}
