using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemySpawner: ObjectSpawner
{
    protected Enemy enemyPrefab;
    protected Enemy enemy;

    protected abstract void SetupEnemy(Enemy enemy, GridObject selectedBlock);

    public override LinkedList<GridObject> FirstSpawn(LinkedList<GridObject> occupiedBlocks)
    {
        GridObject[,] gridObjects = GetEdgeBlocks();
        LinkedList<GridObject> emptyGridObjects = RemoveOccupiedBlocks(gridObjects, occupiedBlocks);
        Vector3 snakeSpawnPosition = snake.GetSpawnPosition();
        LinkedList<GridObject> gridObjectsWithoutSpawnPoint = RemoveSnakeSpawnPoint(snakeSpawnPosition, emptyGridObjects);
        GridObject selectedBlock = PickARandomBlock(gridObjectsWithoutSpawnPoint);
        Vector3 enemyPosition = GenerateObjectPosition(selectedBlock);

        enemy = Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);
        SetupEnemy(enemy, selectedBlock);

        LinkedList<GridObject> newBlocks = new LinkedList<GridObject>();
        newBlocks.AddLast(selectedBlock);

        return newBlocks;
    }

    public override void Spawn(){}


    GridObject[,] GetEdgeBlocks()
    {
        GridObject[,] gridObjects = grid.GetGridObjects();
        int gridSize = grid.GetSize();
        int edgeBlockCount = (2 * gridSize) + (2 * (gridSize - 2));
        GridObject[,] edgeObjects = new GridObject[1, edgeBlockCount];

        int i = 0;
        foreach (var block in gridObjects)
        {
            if (block.Col == 0 || block.Row == 0 || block.Col == (gridSize - 1) || block.Row == (gridSize - 1))
            {
                edgeObjects[0, i++] = block;
            }
        }

        return edgeObjects;
    }
}
