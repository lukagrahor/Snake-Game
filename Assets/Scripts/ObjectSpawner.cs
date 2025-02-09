using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{                                                                                                                                     
    [SerializeField] Food pickup;
    [SerializeField] Snake snake;
    [SerializeField] ArenaBlock arenaBlock;
    [SerializeField] ArenaGrid grid;

    // preveri a se lahko 2 hrane spawnajo na istem mesti
    void Start()
    {
        GridObject[,] gridObjects = grid.GetGridObjects();

        Vector3 snakeSpawnPosition = snake.GetSpawnPosition();
        LinkedList<GridObject> gridObjectsWithoutSpawnPoint = RemoveSnakeSpawnPoint(snakeSpawnPosition, gridObjects);

        Vector3 objectPosition = GenerateObjectPosition(gridObjectsWithoutSpawnPoint);

        Food food = Instantiate(pickup, objectPosition, Quaternion.identity);
        food.SetObjectSpawner(this);
    }

    Vector3 GenerateObjectPosition(LinkedList<GridObject> emptyGridObjects)
    {
        int upperLimit = emptyGridObjects.Count - 1;
        int gridObjectIndex = Random.Range(0, upperLimit);
        Vector3 gridObjectPosition = emptyGridObjects.ElementAt(gridObjectIndex).transform.position;
        Vector3 objectPosition = new Vector3(gridObjectPosition.x, arenaBlock.GetBlockSize() - 0.05f, gridObjectPosition.z);
        return objectPosition;
    }
    
    public void SpawnFood()
    {
        LinkedList<GridObject> emptyGridObjects = GetEmptyGridObjects();
        Vector3 objectPosition = GenerateObjectPosition(emptyGridObjects);
        Food food = Instantiate(pickup, objectPosition, Quaternion.identity);
        food.SetObjectSpawner(this);
    }

    LinkedList<GridObject> GetEmptyGridObjects()
    {
        GridObject[,] gridObjects = grid.GetGridObjects();
        LinkedList<GridObject> emptyGridObjects = new LinkedList<GridObject>();
        foreach (GridObject obj in gridObjects)
        {
            //Debug.Log(obj.name);
            //Debug.Log(obj.isOccupied());
            if (!obj.isOccupied())
            {
                emptyGridObjects.AddLast(obj);
            }
        }
        return emptyGridObjects;
    }
    // To avoid spawning the food on the same spot as the snake, the spawn position is removed from the List of possible spawn locations
    LinkedList<GridObject> RemoveSnakeSpawnPoint(Vector3 snakeSpawnPosition, GridObject[,] gridObjects)
    {
        // the grid block and snake don't have the same y-axis
        snakeSpawnPosition = new Vector3(snakeSpawnPosition.x, arenaBlock.GetBlockSize(), snakeSpawnPosition.z); 
        LinkedList<GridObject> emptyGridObjects = new LinkedList<GridObject>();
        Debug.Log($"snakeSpawnPosition: {snakeSpawnPosition}");
        Debug.Log("gridObjects:");
        Debug.Log(gridObjects[0,0]);
        Debug.Log(gridObjects[0,1]);
        Debug.Log(gridObjects[0,2]);
        foreach (GridObject obj in gridObjects)
        {
            Debug.Log($"Object spawn position: {obj.name}");
            if (obj.transform.position != snakeSpawnPosition)
            {
                emptyGridObjects.AddLast(obj);
            }
        }
        return emptyGridObjects;
    }
    /*
    bool IsNearTheHead()
    {

    }*/

    // Update is called once per frame
    void Update()
    {
        
    }
}
