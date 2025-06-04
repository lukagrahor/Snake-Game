using System.Collections.Generic;
using UnityEngine;

public abstract class EnemySpawner : ObjectSpawner
{
    public abstract void SetupSpawnedEnemy();
    public abstract GridObject[,] GetEdgeBlocks();
}
