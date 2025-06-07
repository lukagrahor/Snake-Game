using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] FoodSpawner foodSpawner;
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] PathSpawner pathSpawner;
    [SerializeField] PlayerSpawner playerSpawner;
    [SerializeField] Bee bee;
    [SerializeField] Wasp wasp;
    [SerializeField] Dog dog;
    [SerializeField] Fly fly;

    List<Enemy> enemies;
    Difficulty currentDifficulty;
    int maxEnemies;
    float spawnDuration = 3f;
   
    public void ManageFirstSpawns(LinkedList<GridObject> wallBlocks)
    {
        LinkedList<GridObject> occupiedBlocks = new LinkedList<GridObject>();
        occupiedBlocks = AddBlocks(occupiedBlocks, wallBlocks);

        LinkedList<GridObject> newBlocks = playerSpawner.FirstSpawn(occupiedBlocks);
        occupiedBlocks = AddBlocks(occupiedBlocks, newBlocks);

        newBlocks = foodSpawner.FirstSpawn(occupiedBlocks);
        occupiedBlocks = AddBlocks(occupiedBlocks, newBlocks);

        currentDifficulty = gameManager.CurrentDifficulty;

        enemies = SetEnemiesToSpawn();
        enemySpawner.WaitForSpawn(enemies);
        /*
        foreach (Enemy enemy in enemies)
        {
            enemySpawner.WaitForSpawn(enemy);
        }*/

        /*
        // spawnanje nasprotnikov rabi prep time + ne smejo se spawnat preveè okuli kaèe, si ne želiš, da gre igralc na nasprotnika med tem ku se ta spawna
        newBlocks = chaseEnemySpawner.FirstSpawn(occupiedBlocks);
        occupiedBlocks = AddBlocks(occupiedBlocks, newBlocks);
        */
    }

    LinkedList<GridObject> AddBlocks(LinkedList<GridObject> occupiedBlocks, LinkedList<GridObject> newBlocks)
    {
        foreach (GridObject block in newBlocks)
        {
            occupiedBlocks.AddLast(block);
        }
        return occupiedBlocks;
    }

    List<Enemy> SetEnemiesToSpawn()
    {
        if (currentDifficulty == Difficulty.Easy) maxEnemies = 5;
        else if (currentDifficulty == Difficulty.Medium) maxEnemies = 8;
        else if (currentDifficulty == Difficulty.Hard) maxEnemies = 12;

        Enemy[] enemyPool = SetEnemyPool();
        List<Enemy> enemyList = new List<Enemy>();
        for (int i = 0; i < maxEnemies; i++)
        {
            int index = Random.Range(0, enemyPool.Length);
            enemyList.Add(enemyPool[index]);
        }

        return enemyList;
    }

    Enemy[] SetEnemyPool() {
        if (currentDifficulty == Difficulty.Easy)
        {
            Enemy[] enemyPool = { bee, dog, fly };
            return enemyPool;
        }
        else if (currentDifficulty == Difficulty.Medium) {
            Enemy[] enemyPool = { bee, wasp, dog, fly };
            return enemyPool;
        }
        else
        {
            Enemy[] enemyPool = { wasp, dog, fly };
            return enemyPool;
        }
    }

    void CheckIfRespawnIsNeeded()
    {

    }
}
