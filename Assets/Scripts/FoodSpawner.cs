using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class FoodSpawner : ObjectSpawner
{
    [SerializeField] Food pickup;
    // preveri a se lahko 2 hrane spawnajo na istem mesti
    void Start()
    {
        Debug.Log("grid");
        Debug.Log(grid);
        GridObject[,] gridObjects = grid.GetGridObjects();

        Vector3 snakeSpawnPosition = snake.GetSpawnPosition();
        Debug.Log("gridObjects:");
        Debug.Log(gridObjects);
        LinkedList<GridObject> gridObjectsWithoutSpawnPoint = RemoveSnakeSpawnPoint(snakeSpawnPosition, gridObjects);

        Vector3 objectPosition = GenerateObjectPosition(gridObjectsWithoutSpawnPoint);

        Food food = Instantiate(pickup, objectPosition, Quaternion.identity);
        food.SetFoodSpawner(this);
    }

    public override void Spawn()
    {
        LinkedList<GridObject> emptyGridObjects = GetEmptyGridObjects();
        Vector3 objectPosition = GenerateObjectPosition(emptyGridObjects);
        Food food = Instantiate(pickup, objectPosition, Quaternion.identity);
        food.SetFoodSpawner(this);
        food.transform.localScale = new Vector3(objectScale, objectScale, objectScale);
    }
}
