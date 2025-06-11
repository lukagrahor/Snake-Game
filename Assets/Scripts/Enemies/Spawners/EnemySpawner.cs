using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class EnemySpawner : ObjectSpawner
{
    /*
    [SerializeField] Enemy enemyPrefab;
    // al nrdi da je generic al pa da je interface
    protected Enemy enemy;*/
    [SerializeField] SpawnIndicator spawnIndicator;
    [SerializeField] SpawnerManager spawnerManager;
    CountDown timer;
    float spawnDuration = 2f;
    Enemy prefab;
    List<SpawnIndicator> indicators;
    Enemy enemyPrefab;

    private void Awake()
    {
        timer = new CountDown(spawnDuration);
        timer.TimeRanOut += Spawn;
        Debug.Log("Èasovnik ajdeeeeeeeeee");
    }
    private void Update()
    {
        Debug.Log("Èasovnik dela");
        timer?.Update();
    }

    public override LinkedList<GridObject> FirstSpawn(LinkedList<GridObject> occupiedBlocks)
    {
        GridObject[,] gridObjects = GetEdgeBlocks();
        LinkedList<GridObject> emptyGridObjects = RemoveOccupiedBlocks(gridObjects, occupiedBlocks);
        Vector3 snakeSpawnPosition = snake.GetSpawnPosition();
        LinkedList<GridObject> gridObjectsWithoutSpawnPoint = RemoveSnakeSpawnPoint(snakeSpawnPosition, emptyGridObjects);
        GridObject selectedBlock = PickARandomBlock(gridObjectsWithoutSpawnPoint);
        Vector3 enemyPosition = GenerateObjectPosition(selectedBlock);

        Enemy enemy = Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);
        SetupEnemy(enemy, selectedBlock);

        LinkedList<GridObject> newBlocks = new LinkedList<GridObject>();
        newBlocks.AddLast(selectedBlock);

        return newBlocks;
    }

    GridObject[,] GetEdgeBlocks()
    {
        GridObject[,] gridObjects = grid.GetGridObjects();
        int gridSize = grid.GetSize();
        int edgeBlockCount = (2 * gridSize) + (2 * (gridSize - 2));
        GridObject[,] edgeObjects = new GridObject[1, edgeBlockCount];

        int i = 0;
        foreach (var block in gridObjects)
        {
            if (block.Col == 0 || block.Row == 0 || block.Col == (gridSize - 1) || block.Row == (gridSize - 1))
            {
                edgeObjects[0, i++] = block;
            }
        }

        return edgeObjects;
    }

    void SetupEnemy(Enemy enemy, GridObject selectedBlock)
    {
        enemy.SetupAI(snake, grid);
        enemy.Setup(selectedBlock.Col, selectedBlock.Row, grid.GetSize());
    }

    public void WaitForSpawn(List<Enemy> enemies)
    {
        indicators = new List<SpawnIndicator>();
        foreach (Enemy enemy in enemies)
        {
            SpawnIndicator indicator = GetPosition(enemy);
            indicators.Add(indicator);
        }
        timer.Timer = spawnDuration;
        timer.Start();
    }

    public SpawnIndicator GetPosition(Enemy enemyPrefab)
    {
        GridObject[,] gridObjects = grid.GetGridObjects();
        LinkedList<GridObject> emptyGridObjects = GetEmptyGridObjects(gridObjects);

        GridObject selectedBlock = PickARandomBlock(emptyGridObjects);
        Vector3 enemyPosition = GenerateObjectPosition(selectedBlock);

        //Indicator newIndicator = new Indicator(enemyPosition, selectedBlock);
        SpawnIndicator indicator = Instantiate(spawnIndicator, enemyPosition, Quaternion.identity);
        indicator.SelectedBlock = selectedBlock;
        indicator.EnemyPrefab = enemyPrefab;
        return indicator;
    }

    public override void Spawn()
    {
        foreach (SpawnIndicator indicator in indicators)
        {
            Enemy enemy = Instantiate(indicator.EnemyPrefab, indicator.transform.position, Quaternion.identity);
            SetupEnemy(enemy, indicator.SelectedBlock);
            enemy.enemyDied += spawnerManager.RespawnEnemies;
        }

        foreach (SpawnIndicator indicator in indicators)
        {
            Debug.Log("Unièenje");
            Destroy(indicator.gameObject);
        }
    }

    public void RemoveAllEnemies()
    {
        // nasprotniki bodo še vedno v FindObjectsByType do konmca tega frame-a
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach(Enemy enemy in enemies)
        {
            Debug.Log("Nasprotnik: " + enemy.name);
            Destroy(enemy.gameObject);
        }
    }
}
