using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class EnemySpawner : BaseEnemySpawner
{
    /*
    [SerializeField] Enemy enemyPrefab;
    // al nrdi da je generic al pa da je interface
    protected Enemy enemy;*/

    protected override void SetupEnemy(Enemy enemy, GridObject selectedBlock)
    {
        enemy.SetupAI(snake, grid);
        enemy.Setup(selectedBlock.Col, selectedBlock.Row, grid.GetSize());
    }
}
