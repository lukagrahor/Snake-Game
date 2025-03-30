using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class EnemySpawner : ObjectSpawner
{
    [SerializeField] TestEnemy enemyPrefab;
    TestEnemy enemy;
    public override LinkedList<GridObject> FirstSpawn(LinkedList<GridObject> occupiedBlocks)
    {
        /* Trenutno je možno, da se nasprotnik pojavi na hrani */
        GridObject[,] gridObjects = GetEdgeBlocks();
        LinkedList<GridObject> emptyGridObjects = RemoveOccupiedBlocks(gridObjects, occupiedBlocks);

        Vector3 snakeSpawnPosition = snake.GetSpawnPosition();
        LinkedList<GridObject> gridObjectsWithoutSpawnPoint = RemoveSnakeSpawnPoint(snakeSpawnPosition, emptyGridObjects);

        GridObject selectedBlock = PickARandomBlock(gridObjectsWithoutSpawnPoint);
        Vector3 enemyPosition = GenerateObjectPosition(selectedBlock);

        enemy = Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);
        enemy.Setup(selectedBlock.Col, selectedBlock.Row, grid.GetSize());

        LinkedList<GridObject> newBlocks = new LinkedList<GridObject>();
        newBlocks.AddLast(selectedBlock);

        return newBlocks;
    }

    public override void Spawn()
    {
        GridObject[,] gridObjects = grid.GetGridObjects();
        LinkedList<GridObject> emptyGridObjects = GetEmptyGridObjects(gridObjects);

        GridObject selectedBlock = PickARandomBlock(emptyGridObjects);
        Vector3 enemyPosition = GenerateObjectPosition(selectedBlock);

        enemy = Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);
        enemy.Setup(selectedBlock.Col, selectedBlock.Row, grid.GetSize());
    }

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
                edgeObjects[0, i] = block;
                i++;
            }
        }
        
        foreach (var block in edgeObjects)
        {
            Debug.Log(block.name);
        }
        return edgeObjects;
    }
}
