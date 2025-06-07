using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class EnemySpawner : BaseEnemySpawner
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

    protected override void SetupEnemy(Enemy enemy, GridObject selectedBlock)
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
    /*
    public void Spawn(Enemy enemyPrefab)
    {
        GridObject[,] gridObjects = grid.GetGridObjects();
        LinkedList<GridObject> emptyGridObjects = GetEmptyGridObjects(gridObjects);

        selectedBlock = PickARandomBlock(emptyGridObjects);
        enemyPosition = GenerateObjectPosition(selectedBlock);

        if (timer == null) timer = new CountDown(spawnDuration);
        else timer.Timer = spawnDuration;
        Debug.Log("Èasovnik zaèni se");
        prefab = enemyPrefab;
        timer.Start();
    }
    */
    /*
    void FinishSpawning()
    {
        Debug.Log("Èasovnik konèaj se");
        enemy = Instantiate(prefab, enemyPosition, Quaternion.identity);
        SetupEnemy(enemy, selectedBlock);
    }
    */
}
