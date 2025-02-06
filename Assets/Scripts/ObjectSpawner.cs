using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] Food pickup;
    [SerializeField] Snake snake;
    [SerializeField] ArenaBlock arenaBlock;
    [SerializeField] ArenaGrid grid;

    LinkedList<GridObject> gridObjects;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gridObjects = grid.GetGridObjects();
        /*
        Vector3 food1Position = new Vector3 (0f, arenaBlock.GetBlockSize() - 0.05f, 0f);
        Vector3 food2Position = new Vector3 (3f, arenaBlock.GetBlockSize() - 0.05f, -2f);
        Vector3 food3Position = new Vector3(2f, arenaBlock.GetBlockSize() - 0.05f, 1f);
        Food food1 = Instantiate(pickup, food1Position, Quaternion.identity);
        Food food2 = Instantiate(pickup, food2Position, Quaternion.identity);
        Food food3 = Instantiate(pickup, food3Position, Quaternion.identity);*/

        //food1.Setup(0f, 3f);
        //food2.Setup(3f, -2f);
        foreach (GridObject obj in gridObjects) {
            Debug.Log(obj.name);
            Debug.Log(obj.isOccupied());
        }
    }
    /*
    void SpawnFood()
    {

    }*/



    // Update is called once per frame
    void Update()
    {
        
    }
}
