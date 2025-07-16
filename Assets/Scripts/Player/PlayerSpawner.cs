using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GlobalEnums;
using System;

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
        Vector3 playerPosition = GenerateObjectPosition(selectedBlocks.First());
        playerPosition.y = 0.185f;

        snake.FirstSpawn(playerPosition);

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

        snake.NewLevelSpawn(playerPosition);

        return selectedBlocks;
    }

    public override void Spawn()
    {
        LinkedList<GridObject> emptyGridObjects = GetEmpty();
        LinkedList<GridObject> selectedBlocks = SelectBlocks(emptyGridObjects);
        Vector3 playerPosition = GenerateObjectPosition(selectedBlocks.First());
        playerPosition.y = 0.185f;
        snake.SnakeHead.transform.position = playerPosition;
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
        bool foundBlock = false;
        LinkedList<GridObject> snakeBlocks = new LinkedList<GridObject>();
        int i = 0;
        while (!foundBlock)
        {
            if (i > 100)
            {
                break;
            }
            i++;
            int upperLimit = emptyGridObjects.Count;
            int gridObjectIndex = UnityEngine.Random.Range(0, upperLimit);

            GridObject gridObject = emptyGridObjects.ElementAt(gridObjectIndex);
            int col = gridObject.Col;
            int row = gridObject.Row;
            Directions dir = snake.StartingDirection;

            if (IsNearAWall(row)) continue;
            snakeBlocks = BoolCheckForOccupiedBlocks(col, row);

            if (snakeBlocks.Count == 0) continue;

            foundBlock = true;
        }

        return snakeBlocks;
    }

    bool IsNearAWall(int cellIndex)
    {
        int gridSize = grid.GetSize();
        int headOffset = cellIndex + distanceFromWall;
        int tailOffset = cellIndex - snakeSize;

        if (headOffset >= gridSize) return true;
        if (tailOffset - snakeSize < 0) return true;
        return false;
    }

    LinkedList<GridObject> BoolCheckForOccupiedBlocks(int col, int row)
    {
        // preveri vsako kocko, kjer naj bi bil trup kaèe, ali je kocka že zasedena
        // ne pozabi med spawnanjem zidov nastavit kocke na occupied
        GridObject[,] gridObjects = grid.GetGridObjects();
        LinkedList<GridObject> snakeBlocks = new LinkedList<GridObject>();
        // spredej
        for (int i = 1; i <= distanceFromWall; i++)
        {
            int currentRow = row + i;
            if (currentRow >= grid.GetSize()) return new LinkedList<GridObject>();
            GridObject obj = gridObjects[col, currentRow];
            if (obj.IsOccupied) return new LinkedList<GridObject>();
        }
        snakeBlocks.AddLast(gridObjects[col, row]);

        // zadej
        for (int i = 1; i <= snakeSize; i++)
        {
            int currentRow = row - i;
            if (currentRow < 0) return new LinkedList<GridObject>();
            GridObject obj = gridObjects[col, currentRow];
            if (obj.IsOccupied) return new LinkedList<GridObject>();
            snakeBlocks.AddLast(obj);
        }
        return snakeBlocks;
    }

    /*

    // preveri ali je plac za trup kaèe in da kaèa ni obrnjena v zid
    bool CheckIfSnakeFitsUp(int cellIndex)
    {
        int gridSize = grid.GetSize();
        Debug.Log("snakeSize 1" + snakeSize);
        if (cellIndex + distanceFromWall >= gridSize) return false;
        if (cellIndex - snakeSize < 0) return false;
        return true;
    }

    bool CheckIfSnakeFitsDown(int cellIndex)
    {
        int gridSize = grid.GetSize();
        Debug.Log("snakeSize 2" + snakeSize);
        if (cellIndex - distanceFromWall < 0) return false;
        if (cellIndex + snakeSize >= gridSize) return false;
        return true;
    }
    */

    /*
    LinkedList<GridObject> BoolCheckForOccupiedBlocksCol(int col, int row, int dir)
    {
        // preveri vsako kocko, kjer naj bi bil trup kaèe, ali je kocka že zasedena
        // ne pozabi med spawnanjem zidov nastavit kocke na occupied
        GridObject[,] gridObjects = grid.GetGridObjects();
        foreach (var item in gridObjects)
        {
            Debug.Log("objektno razmišljanje 1 row:" + item.Row + " col: " + item.Col);
        }
        LinkedList<GridObject> snakeBlocks = new LinkedList<GridObject>();
        snakeBlocks.AddLast(gridObjects[col, row]);
        for (int i = 1; i <= snakeSize; i++)
        {
            GridObject obj = gridObjects[col + (i * dir), row];
            if (obj.IsOccupied) return new LinkedList<GridObject>();
            snakeBlocks.AddLast(obj);
        }
        return snakeBlocks;
    }

    LinkedList<GridObject> BoolCheckForOccupiedBlocksRow(int col, int row, int dir)
    {
        // preveri vsako kocko, kjer naj bi bil trup kaèe, ali je kocka že zasedena
        // ne pozabi med spawnanjem zidov nastavit kocke na occupied
        GridObject[,] gridObjects = grid.GetGridObjects();
        foreach (var item in gridObjects)
        {
            Debug.Log("objektno razmišljanje 2 row:" + item.Row + " col: " + item.Col);
        }
        LinkedList<GridObject> snakeBlocks = new LinkedList<GridObject>();
        snakeBlocks.AddLast(gridObjects[col, row]);
        for (int i = 1; i <= snakeSize; i++)
        {
            GridObject obj = gridObjects[col, row + (i * dir)];
            if (obj.IsOccupied) return new LinkedList<GridObject>();
            snakeBlocks.AddLast(obj);
        }
        return snakeBlocks;
    }
    */

    void SetSnakeStartingDirection()
    {
        /*
        Array DirectionsValues = Enum.GetValues(typeof(Directions));
        int index = UnityEngine.Random.Range(0, DirectionsValues.Length);
        Directions dir = (Directions)DirectionsValues.GetValue(index);
        snake.StartingDirection = dir;
        */
        snake.StartingDirection = Directions.Up;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
