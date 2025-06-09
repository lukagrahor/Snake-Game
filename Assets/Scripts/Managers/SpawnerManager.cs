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
    int minEnemies;
    int maxEnemies;
    int currentEnemies;
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
        currentEnemies = 0;

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

    public void RespawnEnemies()
    {
        currentEnemies--;
        // respawn only if there aren't enough enemies
        Debug.Log("Respawnacija zaèetek");
        Debug.Log("Respawnacija currentEnemies " + currentEnemies);
        Debug.Log("Respawnacija minEnemies " + minEnemies);
        if (currentEnemies > minEnemies) return;
        Debug.Log("Respawnacija treba je lwatet");
        enemies = SetEnemiesToSpawn();
        enemySpawner.WaitForSpawn(enemies);
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
        if (currentDifficulty == Difficulty.Easy)
        {
            minEnemies = 1;
            maxEnemies = 4;
        }
        else if (currentDifficulty == Difficulty.Medium)
        {
            minEnemies = 2;
            maxEnemies = 8;
        }
        else if (currentDifficulty == Difficulty.Hard)
        {
            minEnemies = 4;
            maxEnemies = 12;
        }
        int enemiesToSpawn = maxEnemies - currentEnemies;

        Enemy[] enemyPool = SetEnemyPool();
        List<Enemy> enemyList = new List<Enemy>();
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int index = Random.Range(0, enemyPool.Length);
            enemyList.Add(enemyPool[index]);
        }

        currentEnemies += enemiesToSpawn;

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
