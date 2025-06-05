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
    /*
    void Start()
    {
        ManageFirstSpawns();
    }*/

    public void ManageFirstSpawns(LinkedList<GridObject> wallBlocks)
    {
        LinkedList<GridObject> occupiedBlocks = new LinkedList<GridObject>();
        occupiedBlocks = AddBlocks(occupiedBlocks, wallBlocks);

        LinkedList<GridObject> newBlocks = playerSpawner.FirstSpawn(occupiedBlocks);
        occupiedBlocks = AddBlocks(occupiedBlocks, newBlocks);

        newBlocks = foodSpawner.FirstSpawn(occupiedBlocks);
        occupiedBlocks = AddBlocks(occupiedBlocks, newBlocks);

        currentDifficulty = gameManager.CurrentDifficulty;

        enemies = new List<Enemy>();
        enemies.Add(bee);
        enemies.Add(wasp);

        foreach (Enemy enemy in enemies)
        {
            enemySpawner.Spawn(enemy);
        }
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
        return new List<Enemy>();
    }

    void Update()
    {
        
    }
}
