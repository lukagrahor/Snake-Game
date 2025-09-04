using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GlobalEnums;
using System;
using static UnityEngine.Rendering.DebugUI.Table;

public class PlayerSpawner : ObjectSpawner
{
    [SerializeField] int distanceFromWall = 2;
    //List<GridObject>SnakePositions = new List<GridObject>();
    int snakeSize;
    public override LinkedList<GridObject> FirstSpawn(LinkedList<GridObject> occupiedBlocks)
    {
        GridObject[,] gridObjects = grid.GetGridObjects();
        LinkedList<GridObject> emptyGridObjects = RemoveOccupiedBlocks(gridObjects, occupiedBlocks); // occupied so zidi
        snakeSize = snake.StartingSize;
        SetSnakeStartingDirection();
        LinkedList<GridObject> selectedBlocks = SelectBlocks(emptyGridObjects);
        foreach (GridObject block in selectedBlocks)
        {
            Debug.Log($"block name: {block.name}");
        }
        Vector3 playerPosition = GenerateObjectPosition(selectedBlocks.First());
        playerPosition.y = 0.185f;

        snake.FirstSpawn(playerPosition, selectedBlocks);  // podaj še ostale izbrane bloke

        return selectedBlocks;
    }

    public LinkedList<GridObject> SpawnForNewLevel(LinkedList<GridObject> occupiedBlocks)
    {
        GridObject[,] gridObjects = grid.GetGridObjects();
        LinkedList<GridObject> emptyGridObjects = RemoveOccupiedBlocks(gridObjects, occupiedBlocks); // occupied so zidi
        snakeSize = snake.NewLevelSize;
        SetSnakeStartingDirection();
        LinkedList<GridObject> selectedBlocks = SelectBlocks(emptyGridObjects);
        Vector3 playerPosition = GenerateObjectPosition(selectedBlocks.First());
        playerPosition.y = 0.185f;

        snake.NewLevelSpawn(playerPosition, selectedBlocks);  // podaj še ostale izbrane bloke

        return selectedBlocks;
    }

    public override void Spawn()
    {
        LinkedList<GridObject> emptyGridObjects = GetEmpty();
        LinkedList<GridObject> selectedBlocks = SelectBlocks(emptyGridObjects);
        Vector3 playerPosition = GenerateObjectPosition(selectedBlocks.First());
        playerPosition.y = 0.185f;
        snake.SnakeHead.transform.position = playerPosition; // podaj še ostale izbrane bloke
    }

    public LinkedList<GridObject> SpawnPlayer()
    {
        LinkedList<GridObject> emptyGridObjects = GetEmpty();
        LinkedList<GridObject> selectedBlocks = SelectBlocks(emptyGridObjects);
        Vector3 playerPosition = GenerateObjectPosition(selectedBlocks.First());
        playerPosition.y = 0.185f;
        snake.SnakeHead.transform.position = playerPosition; // podaj še ostale izbrane bloke
        return selectedBlocks;
        /*
        GridObject selectedBlock = PickARandomBlock(emptyGridObjects);
        Vector3 playerPosition = GenerateObjectPosition(selectedBlock);
        snake.SnakeHead.transform.position = playerPosition;
        */
    }

    protected LinkedList<GridObject> GetEmpty()
    {
        GridObject[,] gridObjects = grid.GetGridObjects();
        LinkedList<GridObject> emptyGridObjects = new LinkedList<GridObject>();
        foreach (GridObject obj in gridObjects)
        {
            if (obj.IsOccupied)
            {
                continue;
            }

            emptyGridObjects.AddLast(obj);
        }
        return emptyGridObjects;
    }

    // vrne vse bloke, ki jih kaèa zasede --> upošteva se njena velikost
    protected LinkedList<GridObject> SelectBlocks(LinkedList<GridObject> emptyGridObjects)
    {
        
        List<GridObject> canSpawnOnBlocks = new();
        foreach (GridObject obj in emptyGridObjects)
        {
            if (!IsBlockFree(obj.Col, obj.Row)) continue;
            canSpawnOnBlocks.Add(obj);
        }

        int index = UnityEngine.Random.Range(0, canSpawnOnBlocks.Count);
        GridObject headBlock = canSpawnOnBlocks[index];
        LinkedList<GridObject> snakeBlocks = GetFreeBlocks(headBlock.Col, headBlock.Row);

        return snakeBlocks;
    }
    /*
    bool IsNearAWall(int cellIndex)
    {
        int gridSize = grid.GetSize();
        int headOffset = cellIndex + distanceFromWall;
        int tailOffset = cellIndex - snakeSize;

        if (headOffset >= gridSize) return true;
        if (tailOffset - snakeSize < 0) return true;
        return false;
    }
    */
    LinkedList<GridObject> GetFreeBlocks(int col, int row)
    {
        // check if there is something already on a block where the snake parts will spawn
        GridObject[,] gridObjects = grid.GetGridObjects();

        GridObject headObj = gridObjects[col, row];
        if (headObj.IsOccupied) return new LinkedList<GridObject>();

        LinkedList<GridObject> snakeBlocks = new LinkedList<GridObject>();

        // front
        for (int i = 1; i <= distanceFromWall; i++)
        {
            int currentRow = row + i;
            if (currentRow >= grid.GetSize()) return new LinkedList<GridObject>();
            GridObject obj = gridObjects[col, currentRow];
            if (obj.IsOccupied) return new LinkedList<GridObject>();
        }
        snakeBlocks.AddLast(headObj);

        // back
        for (int i = 1; i <= snakeSize; i++)
        {
            int currentRow = row - i;
            if (currentRow < 0 && i == 1)
            {
                return new LinkedList<GridObject>();
            }
            if (currentRow < 0)
            {
                snakeBlocks = AddTheSides(col, (currentRow + 1), gridObjects, i, snakeBlocks);
                if (snakeBlocks.Count > 0)
                {
                    return snakeBlocks;
                }
                return new LinkedList<GridObject>();
            }
            GridObject obj = gridObjects[col, currentRow];
            if (obj.IsOccupied) return new LinkedList<GridObject>();
            snakeBlocks.AddLast(obj);
        }
        return snakeBlocks;
    }

    bool IsBlockFree(int col, int row)
    {
        GridObject[,] gridObjects = grid.GetGridObjects();
        GridObject headObj = gridObjects[col, row];
        if (headObj.IsOccupied) return false;
        // front
        for (int i = 1; i <= distanceFromWall; i++)
        {
            int currentRow = row + i;
            if (currentRow >= grid.GetSize()) return false;
            GridObject obj = gridObjects[col, currentRow];
            if (obj.IsOccupied) return false;
        }

        // back
        for (int i = 1; i <= snakeSize; i++)
        {
            int currentRow = row - i;
            if (currentRow < 0 && i == 1)
            {
                return false;
            }
            if (currentRow < 0)
            {
                if (CheckTheSides(col, (currentRow + 1), gridObjects, i)) return true;
                return false;
            }
            GridObject obj = gridObjects[col, currentRow];
            if (obj.IsOccupied) return false;
        }
        return true;
    }

    bool CheckTheSides(int col, int row, GridObject[,] gridObjects, int i)
    {
        // first check if the blocks left of the last one can fit
        int colOffset = 1;
        while (i <= snakeSize)
        {
            int currentCol = col - colOffset;
            if (currentCol < 0)
            {
                return false;
            }
            GridObject obj = gridObjects[currentCol, row];
            if (obj.IsOccupied) return false;
            i++;
            colOffset++;
        }
        return true;
        // if not check the right side
    }

    LinkedList<GridObject> AddTheSides(int col, int row, GridObject[,] gridObjects, int i, LinkedList<GridObject> snakeBlocks)
    {
        // first check if the blocks left of the last one can fit
        int colOffset = 1;
        while (i <= snakeSize)
        {
            int currentCol = col - colOffset;
            if (currentCol < 0)
            {
                return new LinkedList<GridObject>();
            }
            GridObject obj = gridObjects[currentCol, row];
            if (obj.IsOccupied) return new LinkedList<GridObject>();
            snakeBlocks.AddLast(obj);
            i++;
            colOffset++;
        }
        return snakeBlocks;
        // if not check the right side
    }

    void SetSnakeStartingDirection()
    {
        snake.StartingDirection = Directions.Up;
    }
}
