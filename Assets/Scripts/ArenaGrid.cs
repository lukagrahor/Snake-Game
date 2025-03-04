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
    void Awake()
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

            float rowPosition = ((i / size) * blockSize) - 5;
            float colPosition = (i * blockSize) % (size * blockSize);
            Vector3 location = new Vector3(colPosition - 5, blockSize, rowPosition);

            GameObject block = Instantiate(gridObjectPrefab.gameObject, location, Quaternion.identity);
            block.transform.localScale = new Vector3(blockSize + 0.001f, blockSize + 0.001f, blockSize + 0.001f);
            block.transform.SetParent(transform);
            GridObject gridObject = block.GetComponent<GridObject>();

            block.name = i.ToString();

            int column = i % size;
            gridObject.Setup(i, column, row);

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
