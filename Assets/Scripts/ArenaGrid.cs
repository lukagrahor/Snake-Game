using System.Collections.Generic;
using UnityEngine;

public class ArenaGrid : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Arena arena;
    [SerializeField] GridObject gridObjectPrefab;
    LinkedList<GridObject> gridObjects;
    void Start()
    {
        gridObjects = new LinkedList<GridObject>();
        SpawnGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SpawnGrid()
    {
        int size = arena.GetSize();
        float blockSize = arena.GetBlockSize();
        for (int i = 0; i < size * size; i++)
        {
            float colNumber = 0;
            Vector3 location = Vector3.zero;
            if (blockSize < 1f)
            {
                colNumber = (i * blockSize) % (size * blockSize);
                location = new Vector3(colNumber - 5, blockSize, ((i / size) * blockSize) - 5);
            }
            else
            {
                colNumber = i % size;
                location = new Vector3(colNumber - 5, blockSize, (i / size) - 5);
            }
            /*
            int colNumber = i % size;

            Vector3 location = new Vector3(colNumber - 5, 1f, (i / size) - 5);
            //Debug.Log($"i: {i} location: {location}");
            */
            GameObject block = Instantiate(gridObjectPrefab.gameObject, location, Quaternion.identity);
            block.transform.localScale = new Vector3(blockSize + 0.001f, blockSize + 0.001f, blockSize + 0.001f);
            block.transform.SetParent(transform);
            GridObject gridObject = block.GetComponent<GridObject>();
            gridObject.setId(i);
            block.name = i.ToString();
            gridObjects.AddLast(gridObject);
        }
    }

    public LinkedList<GridObject> GetGridObjects()
    {
        return gridObjects;
    }
}
