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
    [SerializeField] PlayerSpawner playerSpawner;
    /*
    void Start()
    {
        ManageFirstSpawns();
    }*/

    public void ManageFirstSpawns()
    {
        LinkedList<GridObject> occupiedBlocks = new LinkedList<GridObject>();
        LinkedList<GridObject> newBlocks = playerSpawner.FirstSpawn(occupiedBlocks);
        occupiedBlocks = AddBlocks(occupiedBlocks, newBlocks);

        newBlocks = foodSpawner.FirstSpawn(occupiedBlocks);
        occupiedBlocks = AddBlocks(occupiedBlocks, newBlocks);

        newBlocks = chaseEnemySpawner.FirstSpawn(occupiedBlocks);
        occupiedBlocks = AddBlocks(occupiedBlocks, newBlocks);
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
