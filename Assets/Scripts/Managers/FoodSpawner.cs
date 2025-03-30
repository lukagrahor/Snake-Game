using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class FoodSpawner : ObjectSpawner
{
    [SerializeField] Food foodPrefab;
    Food food;

    public override LinkedList<GridObject> FirstSpawn(LinkedList<GridObject> occupiedBlocks)
    {
        GridObject[,] gridObjects = grid.GetGridObjects();
        LinkedList<GridObject> emptyGridObjects = RemoveOccupiedBlocks(gridObjects, occupiedBlocks);

        Vector3 snakeSpawnPosition = snake.GetSpawnPosition();
        LinkedList<GridObject> gridObjectsWithoutSpawnPoint = RemoveSnakeSpawnPoint(snakeSpawnPosition, emptyGridObjects);

        GridObject selectedBlock = PickARandomBlock(gridObjectsWithoutSpawnPoint);
        Vector3 objectPosition = GenerateObjectPosition(selectedBlock);

        food = Instantiate(foodPrefab, objectPosition, Quaternion.identity);
        food.ApplyScale();
        food.SetSpawner(this);

        LinkedList<GridObject> newBlocks = new LinkedList<GridObject>();
        newBlocks.AddLast(selectedBlock);

        return newBlocks;
    }

    public override void Spawn()
    {
        LinkedList<GridObject> emptyGridObjects = GetEmptyGridObjects(grid.GetGridObjects());

        GridObject selectedBlock = PickARandomBlock(emptyGridObjects);
        Vector3 objectPosition = GenerateObjectPosition(selectedBlock);

        food.SetNewPosition(objectPosition);
    }
}
