using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GlobalEnums;

public class PlayerSpawner : ObjectSpawner
{
    [SerializeField] int distanceFromWall = 5;
    List<GridObject>SnakePositions = new List<GridObject>();
    public override LinkedList<GridObject> FirstSpawn(LinkedList<GridObject> occupiedBlocks)
    {
        GridObject[,] gridObjects = grid.GetGridObjects();
        LinkedList<GridObject> emptyGridObjects = RemoveOccupiedBlocks(gridObjects, occupiedBlocks); // occupied so zidi
        SetSnakeStartingDirection();
        LinkedList<GridObject> selectedBlocks = SelectBlocks(emptyGridObjects);
        //Vector3 snakeSpawnPosition = snake.GetSpawnPosition();
        //LinkedList<GridObject> gridObjectsWithoutSpawnPoint = RemoveSnakeSpawnPoint(snakeSpawnPosition, emptyGridObjects);
        //GridObject selectedBlock = PickARandomBlock(gridObjectsWithoutSpawnPoint);
        Vector3 playerPosition = GenerateObjectPosition(selectedBlocks.First());

        snake.FirstSpawn(playerPosition);

        return selectedBlocks;
    }

    public override void Spawn()
    {
        /*
        GridObject[,] gridObjects = grid.GetGridObjects();
        LinkedList<GridObject> emptyGridObjects = GetEmptyGridObjects(gridObjects);

        GridObject selectedBlock = PickARandomBlock(emptyGridObjects);
        Vector3 enemyPosition = GenerateObjectPosition(selectedBlock);

        enemy = Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);
        SetupEnemy(enemy, selectedBlock);*/
    }

    // vrne vse bloke, ki jih kaèa zasede --> upošteva se njena velikost
    protected LinkedList<GridObject> SelectBlocks(LinkedList<GridObject> emptyGridObjects)
    {
        bool foundBlock = false;
        LinkedList<GridObject> snakeBlocks = new LinkedList<GridObject>();
        while (!foundBlock)
        {
            int upperLimit = emptyGridObjects.Count - 1;
            int gridObjectIndex = Random.Range(0, upperLimit);

            GridObject gridObject = emptyGridObjects.ElementAt(gridObjectIndex);
            int col = gridObject.Col;
            int row = gridObject.Row;
            Directions dir = snake.SnakeHead.StartingDirection;

            if (dir == Directions.Up)
            { 
                if (!CheckIfSnakeFitsUp(col)) continue;
                snakeBlocks = BoolCheckForOccupiedBlocksCol(col, row, -1);
                if (snakeBlocks.Count == 0) continue;
            }
            else if (dir == Directions.Down)
            {
                if (!CheckIfSnakeFitsDown(col)) continue;
                snakeBlocks = BoolCheckForOccupiedBlocksCol(col, row, 1);
                if (snakeBlocks.Count == 0) continue;
            }
            else if (dir == Directions.Left)
            {
                if (!CheckIfSnakeFitsDown(col)) continue;
                snakeBlocks = BoolCheckForOccupiedBlocksRow(col, row, 1);
                if (snakeBlocks.Count == 0) continue;
            }
            else if (dir == Directions.Right)
            {
                if (!CheckIfSnakeFitsUp(col)) continue;
                snakeBlocks = BoolCheckForOccupiedBlocksRow(col, row, -1);
                if (snakeBlocks.Count == 0) continue;
            }
        }

        return snakeBlocks;
    }

    // preveri ali je plac za trup kaèe in da kaèa ni obrnjena v zid
    bool CheckIfSnakeFitsUp(int cellIndex)
    {
        int gridSize = grid.GetSize();
        int snakeSize = snake.SnakeSize;
        if (cellIndex + distanceFromWall >= gridSize) return false;
        if (cellIndex - snakeSize < 0) return false;
        return true;
    }

    bool CheckIfSnakeFitsDown(int cellIndex)
    {
        int gridSize = grid.GetSize();
        int snakeSize = snake.SnakeSize;
        if (cellIndex - distanceFromWall < 0) return false;
        if (cellIndex + snakeSize >= gridSize) return false;
        return true;
    }

    LinkedList<GridObject> BoolCheckForOccupiedBlocksCol(int col, int row, int dir)
    {
        // preveri vsako kocko, kjer naj bi bil trup kaèe, ali je kocka že zasedena
        // ne pozabi med spawnanjem zidov nastavit kocke na occupied
        GridObject[,] gridObjects = grid.GetGridObjects();
        LinkedList<GridObject> snakeBlocks = new LinkedList<GridObject>();
        snakeBlocks.AddLast(gridObjects[col, row]);
        int snakeSize = snake.SnakeSize;
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
        LinkedList<GridObject> snakeBlocks = new LinkedList<GridObject>();
        snakeBlocks.AddLast(gridObjects[col, row]);
        int snakeSize = snake.SnakeSize;
        for (int i = 1; i <= snakeSize; i++)
        {
            GridObject obj = gridObjects[col, row + (i * dir)];
            if (obj.IsOccupied) return new LinkedList<GridObject>();
            snakeBlocks.AddLast(obj);
        }
        return snakeBlocks;
    }

    void SetSnakeStartingDirection()
    {
        int directonIndex = Random.Range(0, 4);
        Directions dir = (Directions)Random.Range(0, 4);
        snake.SnakeHead.StartingDirection = dir;
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
