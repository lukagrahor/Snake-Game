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

        selectedBlock.Food = food;
        food.LocationObject = selectedBlock;
        grid.ObjectsWithFood.Add(selectedBlock);

        foreach (GridObject gridObject in grid.ObjectsWithFood)
        {
            Debug.Log("objekt s hrano " + gridObject.name);
        }

        return newBlocks;
    }

    public override void Spawn()
    {
        LinkedList<GridObject> emptyGridObjects = GetEmptyGridObjects(grid.GetGridObjects());

        GridObject selectedBlock = PickARandomBlock(emptyGridObjects);
        Vector3 objectPosition = GenerateObjectPosition(selectedBlock);

        food.SetNewPosition(objectPosition);
        selectedBlock.Food = food;
        food.LocationObject = selectedBlock;
        grid.ObjectsWithFood.Add(selectedBlock);

        foreach (GridObject gridObject in grid.ObjectsWithFood)
        {
            Debug.Log("objekt s hrano " + gridObject.name);
        }
    }

    public void RemovePreviousObject(GridObject locationObject)
    {
        List<GridObject> objectsWithFood = grid.ObjectsWithFood;
        int i = 0;
        while(i < objectsWithFood.Count)
        {
            GridObject gridObject = objectsWithFood[i];
            if (locationObject.name == gridObject.name)
            {
                objectsWithFood.Remove(gridObject);
                continue;
            }
            i++;
        }
    }
}
