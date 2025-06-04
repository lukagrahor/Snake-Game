using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] FoodSpawner foodSpawner;
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] StationaryEnemySpawner stationaryEnemySpawner;
    [SerializeField] AIEnemySpawner chaseEnemySpawner;
    [SerializeField] DogSpawner dogSpawner;
    [SerializeField] PathSpawner pathSpawner;
    [SerializeField] WaspSpawner waspSpawner;
    [SerializeField] PlayerSpawner playerSpawner;
    Difficulty difficulty;
    int maxEnemies;
    public List<EnemySpawner> enemySpawners;
    /*
    void Start()
    {
        ManageFirstSpawns();
    }*/

    void Start()
    {
        //difficulty = gameManager.CurrentDifficulty;
        //enemySpawners.Add(waspSpawner);
        //enemySpawners.Add(dogSpawner);
    }

    public void ManageFirstSpawns(LinkedList<GridObject> wallBlocks)
    {
        LinkedList<GridObject> occupiedBlocks = new LinkedList<GridObject>();
        occupiedBlocks = AddBlocks(occupiedBlocks, wallBlocks);

        LinkedList<GridObject> newBlocks = playerSpawner.FirstSpawn(occupiedBlocks);
        occupiedBlocks = AddBlocks(occupiedBlocks, newBlocks);

        newBlocks = foodSpawner.FirstSpawn(occupiedBlocks);
        occupiedBlocks = AddBlocks(occupiedBlocks, newBlocks);

        foreach (EnemySpawner spawner in enemySpawners)
        {
            spawner.Spawn();
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


    void Update()
    {
        
    }
}
