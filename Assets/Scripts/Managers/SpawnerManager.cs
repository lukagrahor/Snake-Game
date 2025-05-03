using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] FoodSpawner foodSpawner;
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] StationaryEnemySpawner stationaryEnemySpawner;
    [SerializeField] AIEnemySpawner chaseEnemySpawner;
    [SerializeField] DogSpawner dogSpawner;
    [SerializeField] PathSpawner pathSpawner;
    [SerializeField] WaspSpawner waspSpawner;
    void Start()
    {
        ManageFirstSpawns();
    }

    void ManageFirstSpawns()
    {
        LinkedList<GridObject> occupiedBlocks = new LinkedList<GridObject>();
        LinkedList<GridObject> newBlocks = foodSpawner.FirstSpawn(occupiedBlocks);

        occupiedBlocks = AddBlocks(occupiedBlocks, newBlocks);
        newBlocks = enemySpawner.FirstSpawn(occupiedBlocks);
        occupiedBlocks = AddBlocks(occupiedBlocks, newBlocks);
        //Debug.Log("Occupied 1: " + occupiedBlocks.Count);
        newBlocks = stationaryEnemySpawner.FirstSpawn(occupiedBlocks);
        occupiedBlocks = AddBlocks(occupiedBlocks, newBlocks);
        //Debug.Log("Occupied 2: " + occupiedBlocks.Count);
        newBlocks = chaseEnemySpawner.FirstSpawn(occupiedBlocks);
        occupiedBlocks = AddBlocks(occupiedBlocks, newBlocks);
        newBlocks = dogSpawner.FirstSpawn(occupiedBlocks);
        occupiedBlocks = AddBlocks(occupiedBlocks, newBlocks);
        newBlocks = waspSpawner.FirstSpawn(occupiedBlocks);
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
