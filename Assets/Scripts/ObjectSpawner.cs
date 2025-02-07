using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{                                                                                                                                     
    [SerializeField] Food pickup;
    [SerializeField] Snake snake;
    [SerializeField] ArenaBlock arenaBlock;
    [SerializeField] ArenaGrid grid;

    LinkedList<GridObject> gridObjects;

    // preveri a se lahko 2 hrane spawnajo na istem mesti
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
        /*
        foreach (GridObject obj in gridObjects) {
            Debug.Log(obj.name);
            Debug.Log(obj.isOccupied());
        }*/
        //Instantiate(pickup, );
        Vector3 objectPosition = GenerateObjectPosition(gridObjects);

        Food food = Instantiate(pickup, objectPosition, Quaternion.identity);
        food.SetObjectSpawner(this);
    }

    Vector3 GenerateObjectPosition(LinkedList<GridObject> emptyGridObjects)
    {
        int upperLimit = emptyGridObjects.Count - 1;
        int gridObjectIndex = Random.Range(0, upperLimit);
        Vector3 gridObjectPosition = emptyGridObjects.ElementAt(gridObjectIndex).transform.position;
        Vector3 objectPosition = new Vector3(gridObjectPosition.x, arenaBlock.GetBlockSize() - 0.05f, gridObjectPosition.z);
        return objectPosition;
    }
    
    public void SpawnFood()
    {
        LinkedList<GridObject> emptyGridObjects = GetEmptyGridObjects();
        Vector3 objectPosition = GenerateObjectPosition(emptyGridObjects);
        Food food = Instantiate(pickup, objectPosition, Quaternion.identity);
        food.SetObjectSpawner(this);
    }

    LinkedList<GridObject> GetEmptyGridObjects()
    {
        LinkedList<GridObject> emptyGridObjects = new LinkedList<GridObject>();
        foreach (GridObject obj in gridObjects)
        {
            //Debug.Log(obj.name);
            //Debug.Log(obj.isOccupied());
            if (!obj.isOccupied())
            {
                emptyGridObjects.AddLast(obj);
            }
        }
        return emptyGridObjects;
    }

        // Update is called once per frame
    void Update()
    {
        
    }
}
