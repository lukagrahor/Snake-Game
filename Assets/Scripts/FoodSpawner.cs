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

        Vector3 objectPosition = GenerateObjectPosition(gridObjectsWithoutSpawnPoint);

        food = Instantiate(foodPrefab, objectPosition, Quaternion.identity);
        Debug.Log($"food po instaciranju: {food}");
        food.SetFoodSpawner(this);
    }

    public override void Spawn()
    {
        LinkedList<GridObject> emptyGridObjects = GetEmptyGridObjects();
        Vector3 objectPosition = GenerateObjectPosition(emptyGridObjects);
        Debug.Log($"food: {food}");
        food.SetNewPosition(objectPosition);
        Debug.Log($"Food active: {gameObject.activeSelf}");
    }
}
