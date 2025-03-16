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
        for (int i = 0; i < size * size; i++)
        {
            int row = i / size;
            int column = i % size;

            int blockPositionOffset = 5;
            float rowPosition = ((i / size) * blockSize) - blockPositionOffset;
            float colPosition = (i * blockSize) % (size * blockSize);
            Vector3 location = new Vector3(colPosition - blockPositionOffset, blockSize, rowPosition);

            GameObject block = Instantiate(gridObjectPrefab.gameObject, location, Quaternion.identity);
            block.transform.localScale = new Vector3(blockSize, blockSize, blockSize);
            block.transform.SetParent(transform);
            GridObject gridObject = block.GetComponent<GridObject>();

            block.name = i.ToString();
            gridObject.Col = column;
            gridObject.Row = row;

            gridObjects[column, row] = gridObject;
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
