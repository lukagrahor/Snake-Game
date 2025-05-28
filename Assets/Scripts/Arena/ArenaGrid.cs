using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class ArenaGrid : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Arena arena;
    [SerializeField] GridObject gridObjectPrefab;
    GridObject [,] gridObjects;
    List<GridObject> objectsWithFood;
    public List<GridObject> ObjectsWithFood { get => objectsWithFood; set => objectsWithFood = value; }
    int size;
    void Awake()
    {
        size = arena.GetSize();
        gridObjects = new GridObject[size, size];
        objectsWithFood = new List<GridObject>();
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

    public List<GridObject> GetNeighbours(GridObject gObject)
    {
        List<GridObject> neighbours = new List<GridObject>();
        int col = gObject.Col;
        int row = gObject.Row;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if ((i == 0 && j == 0) || (i == -1 && j == -1) || (i == 1 && j == 1) || (i == 1 && j == -1) || (i == -1 && j == 1)) continue;

                int newCol = gObject.Col + j;
                int newRow = gObject.Row + i;

                // èe je izven dosega arene
                if (newCol < 0 || newCol >= size || newRow < 0 || newRow >= size) continue;

                GridObject neighbour = gridObjects[newCol, newRow];
                neighbours.Add(neighbour);
            }
        }
        
        return neighbours;
    }


    public void SpawnWalls(List<IntPair> wallLocations)
    {
        foreach (var item in wallLocations)
        {
            Debug.Log("wall location, Col: " + item.Col + ", Row: " + item.Row);
        }
    }
}
