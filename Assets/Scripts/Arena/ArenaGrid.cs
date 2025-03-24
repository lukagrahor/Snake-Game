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

    void SpawnGrid()
    {
        int size = arena.GetSize();
        float blockSize = arena.GetBlockSize();

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float blockPositionOffset = 5f;
                float rowPosition = (i * blockSize) - blockPositionOffset;
                float colPosition = (j * blockSize) - blockPositionOffset;
                Vector3 location = new(colPosition, blockSize, rowPosition);

                GameObject block = Instantiate(gridObjectPrefab.gameObject, location, Quaternion.identity);
                block.transform.localScale = new Vector3(blockSize, blockSize, blockSize);
                block.transform.SetParent(transform);
                GridObject gridObject = block.GetComponent<GridObject>();

                block.name =$"Row: {i} Col: {j}";
                gridObject.Col = j;
                gridObject.Row = i;

                gridObjects[j, i] = gridObject;

            }
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
