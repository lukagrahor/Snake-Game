using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class ObjectSpawner : MonoBehaviour
{
    [SerializeField] protected Snake snake;
    [SerializeField] protected ArenaBlock arenaBlock;
    [SerializeField] protected ArenaGrid grid;

    [SerializeField] protected float objectScale = 0.4f;

    public abstract LinkedList<GridObject> FirstSpawn(LinkedList<GridObject> occupiedBlocks);
    public abstract void Spawn();

    protected GridObject PickARandomBlock(LinkedList<GridObject> emptyGridObjects)
    {
        int upperLimit = emptyGridObjects.Count - 1;
        int gridObjectIndex = Random.Range(0, upperLimit);
        Debug.Log("gridObjectIndex: " + gridObjectIndex);
        Debug.Log("gridObjectIndex: " + emptyGridObjects.Count);
        foreach(GridObject gO in emptyGridObjects)
        {
            Debug.Log("objekt: " + gO.name);
        }
        GridObject gridObject = emptyGridObjects.ElementAt(gridObjectIndex); // tle uèasih gre out of range --> prazn emptyGridObjects
        return gridObject;
    }

    protected Vector3 GenerateObjectPosition(GridObject gridObject)
    {
        Vector3 gridObjectPosition = gridObject.transform.position;
        float yPosition = arenaBlock.GetBlockSize() / 2f + objectScale / 2f;
        Vector3 objectPosition = new (gridObjectPosition.x, yPosition, gridObjectPosition.z);
        return objectPosition;
    }
    protected LinkedList<GridObject> GetEmptyGridObjects(GridObject[,] gridObjects)
    {
        LinkedList<GridObject> noSpawnBlocks = GetBlocksAtTheHead(gridObjects);
        LinkedList<GridObject> emptyGridObjects = new LinkedList<GridObject>();
        foreach (GridObject obj in gridObjects)
        {
            if (obj.IsOccupied)
            {
                continue;
            }
            if (IsNextToHead(noSpawnBlocks, obj) == true)
            {
                continue;
            }
            emptyGridObjects.AddLast(obj);
        }
        return emptyGridObjects;
    }
    /// <summary>
    /// A method used for the first spawn of objects. At the start different spawners each spawn their objects and to prevent overlap we use this method.
    /// </summary>
    protected LinkedList<GridObject> RemoveOccupiedBlocks(GridObject[,] gridObjects, LinkedList<GridObject> occupiedBlocks)
    {
        LinkedList<GridObject> emptyBlocks = new LinkedList<GridObject>();
        bool isOccupied = false;
        foreach (GridObject obj in gridObjects)
        {
            foreach (GridObject occupied in occupiedBlocks)
            {
                if (obj.name == occupied.name)
                {
                    isOccupied = true;
                }
            }
            if (!isOccupied)
            {
                emptyBlocks.AddLast(obj);
            }
        }
        return emptyBlocks;
    }

    protected bool IsNextToHead(LinkedList<GridObject> noSpawnBlocks, GridObject obj)
    {
        foreach (GridObject noSpawnBlock in noSpawnBlocks)
        {
            if (obj.name == noSpawnBlock.name)
            {
                return true;
            }
        }
        return false;
    }

    protected LinkedList<GridObject> GetBlocksAtTheHead(GridObject[,] gridObjects)
    {
        LinkedList<GridObject> emptyGridObjects = new LinkedList<GridObject>();
        LinkedList<GridObject> occupiedByHead = new LinkedList<GridObject>();

        foreach (GridObject obj in gridObjects)
        {
            if (obj.IsOccupiedBySnakeHead)
            {
                occupiedByHead.AddLast(obj);
            }
        }

        if (occupiedByHead.Count == 0)
        {
            return new LinkedList<GridObject>();
        }

        GridObject headPositionBlock = GetHeadPositionBlock(occupiedByHead);

        // odstrani sosednje kocke
        LinkedList<GridObject> noSpawnBlocks = new LinkedList<GridObject>();
        int col = headPositionBlock.Col;
        int row = headPositionBlock.Row;
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

    protected GridObject GetHeadPositionBlock(LinkedList<GridObject> occupiedByHead)
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
    protected LinkedList<GridObject> RemoveSnakeSpawnPoint(Vector3 snakeSpawnPosition, LinkedList<GridObject> gridObjects)
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
}
