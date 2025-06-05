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

    public void ManageFirstSpawns(LinkedList<GridObject> wallBlocks)
    {
        LinkedList<GridObject> occupiedBlocks = new LinkedList<GridObject>();
        occupiedBlocks = AddBlocks(occupiedBlocks, wallBlocks);

        LinkedList<GridObject> newBlocks = playerSpawner.FirstSpawn(occupiedBlocks);
        occupiedBlocks = AddBlocks(occupiedBlocks, newBlocks);

        newBlocks = foodSpawner.FirstSpawn(occupiedBlocks);
        occupiedBlocks = AddBlocks(occupiedBlocks, newBlocks);
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
