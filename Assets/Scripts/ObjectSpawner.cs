using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] Food pickup;
    [SerializeField] Snake snake;
    [SerializeField] ArenaBlock arenaBlock;
    [SerializeField] ArenaGrid grid;

    [SerializeField] float objectScale = 0.4f;

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
        float yPosition = arenaBlock.GetBlockSize() / 2f + objectScale / 2f;
        Vector3 objectPosition = new Vector3(gridObjectPosition.x, yPosition, gridObjectPosition.z);
        return objectPosition;
    }

    public void SpawnFood()
    {
        LinkedList<GridObject> emptyGridObjects = GetEmptyGridObjects();
        Vector3 objectPosition = GenerateObjectPosition(emptyGridObjects);
        Food food = Instantiate(pickup, objectPosition, Quaternion.identity);
        food.SetObjectSpawner(this);
        food.transform.localScale = new Vector3(objectScale, objectScale, objectScale);
    }

    LinkedList<GridObject> GetEmptyGridObjects()
    {
        GridObject[,] gridObjects = grid.GetGridObjects();
        LinkedList<GridObject> noSpawnBlocks = GetBlocksAtTheHead(gridObjects);
        LinkedList<GridObject> emptyGridObjects = new LinkedList<GridObject>();
        foreach (GridObject obj in gridObjects)
        {
            if (IsNextToHead(noSpawnBlocks, obj) == true)
            {
                continue;
            }
            if (!obj.isOccupied())
            {
                emptyGridObjects.AddLast(obj);
            }
        }
        return emptyGridObjects;
    }

    bool IsNextToHead(LinkedList<GridObject> noSpawnBlocks, GridObject obj)
    {
        foreach (GridObject noSpawnBlock in noSpawnBlocks)
        {
            if (obj.name == noSpawnBlock.name) {
                return true;
            }
        }
        return false;
    }

    LinkedList<GridObject> GetBlocksAtTheHead(GridObject[,] gridObjects)
    {
        LinkedList<GridObject> emptyGridObjects = new LinkedList<GridObject>();
        LinkedList<GridObject> occupiedByHead = new LinkedList<GridObject>();

        foreach (GridObject obj in gridObjects)
        {
            if (obj.IsOccupiedBySnakehead())
            {
                occupiedByHead.AddLast(obj);
            }
        }

        GridObject headPositionBlock = GetHeadPositionBlock(occupiedByHead);

        // odstrani sosednje kocke
        LinkedList<GridObject> noSpawnBlocks = new LinkedList<GridObject>();
        int col = headPositionBlock.GetCol();
        int row = headPositionBlock.GetRow();
        int gridSize = grid.GetSize();
        if (col != gridSize - 1)
        {
            noSpawnBlocks.AddLast(gridObjects[col + 1, row]);
        }
        if (col != 0)
        {
            noSpawnBlocks.AddLast(gridObjects[col - 1, row]);
        }
        if (row != gridSize - 1)
        {
            noSpawnBlocks.AddLast(gridObjects[col, row + 1]);
        }
        if (row != 0)
        {
            noSpawnBlocks.AddLast(gridObjects[col, row - 1]);
        }

        return noSpawnBlocks;
    }

    GridObject GetHeadPositionBlock(LinkedList<GridObject> occupiedByHead)
    {
        int snakeDirection = (int)snake.GetSnakeYRotation();
        // rotacija 0: col --> +
        // rotacija 90: row --> +
        // rotacija 180: col --> -
        // rotacija 270: row --> -
        GridObject headPositionBlock = null;
        if (snakeDirection == 0 || snakeDirection == 90)
        {
            headPositionBlock = occupiedByHead.Last();
        }
        else if (snakeDirection == 180 || snakeDirection == 270)
        {
            headPositionBlock = occupiedByHead.First();
        }

        return headPositionBlock;
    }
    // To avoid spawning the food on the same spot as the snake, the spawn position is removed from the List of possible spawn locations
    LinkedList<GridObject> RemoveSnakeSpawnPoint(Vector3 snakeSpawnPosition, GridObject[,] gridObjects)
    {
        // the grid block and snake don't have the same y-axis
        snakeSpawnPosition = new Vector3(snakeSpawnPosition.x, arenaBlock.GetBlockSize(), snakeSpawnPosition.z); 
        LinkedList<GridObject> emptyGridObjects = new LinkedList<GridObject>();
        foreach (GridObject obj in gridObjects)
        {
            if (obj.transform.position != snakeSpawnPosition)
            {
                emptyGridObjects.AddLast(obj);
            }
        }
        return emptyGridObjects;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
