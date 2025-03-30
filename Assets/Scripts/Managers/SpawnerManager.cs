using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] FoodSpawner foodSpawner;
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] StationaryEnemySpawner stationaryEnemySpawner;
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
        newBlocks = stationaryEnemySpawner.FirstSpawn(occupiedBlocks);
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
