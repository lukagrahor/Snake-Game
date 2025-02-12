using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class ArenaGrid : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Arena arena;
    [SerializeField] GridObject gridObjectPrefab;
    GridObject [,] gridObjects;
    int size;
    void Start()
    {
        size = arena.GetSize();
        gridObjects = new GridObject[size, size];
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
        int row = 0;
        int j = 0;
        for (int i = 0; i < size * size; i++)
        {
            if (j == size)
            {
                row++;
                j = 0;
            }
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

            block.name = i.ToString();

            int column = i % size;
            gridObject.Setup(i, column, row);
            Debug.Log($"Column:{column},  Row: {row}, objekt: {gridObject.name}");

            gridObjects[column, row] = gridObject;
            j++;
        }
    }

    public GridObject[,] GetGridObjects()
    {
        return gridObjects;
    }

    public int GetSize()
    {
        return size;
    }
}
