using System.Collections.Generic;
using UnityEngine;

public class StationaryEnemySpawner : ObjectSpawner
{
    [SerializeField] StationaryEnemy enemyPrefab;
    StationaryEnemy enemy;
    public override LinkedList<GridObject> FirstSpawn(LinkedList<GridObject> occupiedBlocks)
    {
        GridObject[,] gridObjects = grid.GetGridObjects();
        LinkedList<GridObject> emptyGridObjects = RemoveOccupiedBlocks(gridObjects, occupiedBlocks);

        Vector3 snakeSpawnPosition = snake.GetSpawnPosition();
        LinkedList<GridObject> gridObjectsWithoutSpawnPoint = RemoveSnakeSpawnPoint(snakeSpawnPosition, emptyGridObjects);

        GridObject selectedBlock = PickARandomBlock(gridObjectsWithoutSpawnPoint);
        Vector3 enemyPosition = GenerateObjectPosition(selectedBlock);

        enemy = Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);

        LinkedList<GridObject> newBlocks = new LinkedList<GridObject>();
        newBlocks.AddLast(selectedBlock);

        return newBlocks;
    }

    public override void Spawn()
    {
        LinkedList<GridObject> emptyGridObjects = GetEmptyGridObjects(grid.GetGridObjects());

        GridObject selectedBlock = PickARandomBlock(emptyGridObjects);
        Vector3 enemyPosition = GenerateObjectPosition(selectedBlock);

        enemy = Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);
    }
}
