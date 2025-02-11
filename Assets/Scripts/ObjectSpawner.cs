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
        //gridObjects = RemoveBlocksAtTheHead(gridObjects);
        LinkedList<GridObject> noSpawnBlocks = RemoveBlocksAtTheHead(gridObjects);
        LinkedList<GridObject> emptyGridObjects = new LinkedList<GridObject>();
        foreach (GridObject obj in gridObjects)
        {
            //Debug.Log(obj.name);
            //Debug.Log(obj.isOccupied());
            if (isNextToHead(noSpawnBlocks, obj))
            {
                Debug.Log("Juup"); // hmmm samu enkrat reèe juup, moglu bi 4-krat
                continue;
            }
            if (!obj.isOccupied())
            {
                emptyGridObjects.AddLast(obj);
            }
        }
        foreach (GridObject obj in emptyGridObjects)
        {
            Debug.Log($"emptyGridObjects {obj.name}, kol:{obj.GetCol()}, rol:{obj.GetRow()}");
        }
        return emptyGridObjects;
    }

    bool isNextToHead(LinkedList<GridObject> noSpawnBlocks, GridObject obj)
    {
        foreach (GridObject noSpawnBlock in noSpawnBlocks)
        {
            if (obj.name == noSpawnBlock.name) {
                return true;
            }
        }
        return false;
    }

    LinkedList<GridObject> RemoveBlocksAtTheHead(GridObject[,] gridObjects)
    {
        LinkedList<GridObject> emptyGridObjects = new LinkedList<GridObject>();
        LinkedList<GridObject> occupiedByHead = new LinkedList<GridObject>();

        foreach (GridObject obj in gridObjects)
        {
            //Debug.Log(obj.name);
            //Debug.Log(obj.isOccupied());
            if (obj.IsOccupiedBySnakehead())
            {
                Debug.Log($"Glava je tu: {obj.name}");
            }

            if (obj.IsOccupiedBySnakehead())
            {
                occupiedByHead.AddLast(obj);
            }
        }
        Debug.Log($"golaž: {gridObjects.GetLength(0)}");

        float moveDirection = snake.GetSnakeYRotation();
        // kaj pa èe je kocka v koti al pr robi
        Debug.Log($"moveDirection: {moveDirection}");
        // rotacija 0: col --> +
        // rotacija 90: row --> +
        // rotacija 180: col --> -
        // rotacija 270: row --> -
        foreach (GridObject obj in occupiedByHead)
        {
            Debug.Log($"kol:{obj.GetCol()}, rol:{obj.GetRow()}");
        }
        GridObject headPositionBlock = null;
        if (moveDirection == 0 || moveDirection == 90)
        {
            headPositionBlock = occupiedByHead.Last();
        }
        else if (moveDirection == 180 || moveDirection == 270)
        {
            headPositionBlock = occupiedByHead.First();
        }
        // odstrani sosednje kocke
        LinkedList<GridObject> noSpawnBlocks = new LinkedList<GridObject>();
        Debug.Log($"Izbrani --> kol:{headPositionBlock.GetCol()}, rol:{headPositionBlock.GetRow()}");
        int col = headPositionBlock.GetCol();
        int row = headPositionBlock.GetRow();
        if (col != arenaBlock.GetBlockSize() - 1)
        {
            noSpawnBlocks.AddLast(gridObjects[col + 1, row]);
        }
        if (col != 0)
        {
            noSpawnBlocks.AddLast(gridObjects[col - 1, row]);
        }
        if (row != arenaBlock.GetBlockSize() - 1)
        {
            noSpawnBlocks.AddLast(gridObjects[col, row + 1]);
        }
        if (row != 0)
        {
            noSpawnBlocks.AddLast(gridObjects[col, row - 1]);
        }

        foreach (GridObject obj in noSpawnBlocks)
        {
            Debug.Log($"noSpawnBlocks {obj.name}, kol:{obj.GetCol()}, rol:{obj.GetRow()}");
        }

        return noSpawnBlocks;
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
