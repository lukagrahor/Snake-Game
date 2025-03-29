using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class FoodSpawner : ObjectSpawner
{
    [SerializeField] Food foodPrefab;
    Food food;
    void Start()
    {
        GridObject[,] gridObjects = grid.GetGridObjects();

        Vector3 snakeSpawnPosition = snake.GetSpawnPosition();
        LinkedList<GridObject> gridObjectsWithoutSpawnPoint = RemoveSnakeSpawnPoint(snakeSpawnPosition, gridObjects);

        GridObject selectedBlock = PickARandomBlock(gridObjectsWithoutSpawnPoint);
        Vector3 objectPosition = GenerateObjectPosition(selectedBlock);

        food = Instantiate(foodPrefab, objectPosition, Quaternion.identity);
        food.ApplyScale();
        food.SetSpawner(this);
    }

    public override void Spawn()
    {
        LinkedList<GridObject> emptyGridObjects = GetEmptyGridObjects(grid.GetGridObjects());

        GridObject selectedBlock = PickARandomBlock(emptyGridObjects);
        Vector3 objectPosition = GenerateObjectPosition(selectedBlock);

        food.SetNewPosition(objectPosition);
    }
}
