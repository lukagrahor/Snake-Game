using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class EnemySpawner : BaseEnemySpawner
{
    /*
    [SerializeField] Enemy enemyPrefab;
    // al nrdi da je generic al pa da je interface
    protected Enemy enemy;*/
    CountDown timer;
    float spawnDuration = 3f;

    Vector3 enemyPosition;
    GridObject selectedBlock;
    Enemy prefab;
    private void Awake()
    {
        timer = new CountDown(spawnDuration);
        timer.TimeRanOut += FinishSpawning;
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

    void FinishSpawning()
    {
        Debug.Log("Èasovnik konèaj se");
        enemy = Instantiate(prefab, enemyPosition, Quaternion.identity);
        SetupEnemy(enemy, selectedBlock);
    }
}
