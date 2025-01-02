using UnityEngine;

public class Grid : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Arena arena;
    [SerializeField] GridObject gridObject;
    void Start()
    {
        SpawnGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SpawnGrid()
    {
        Vector3 bottom = Vector3.zero;
        Vector3 left = Vector3.zero;
        Vector3 right = Vector3.zero;
        Vector3 top = Vector3.zero;
        int colNumber = 0;
        int size = arena.GetSize();
        for (int i = 0; i < size * size; i++)
        {
            colNumber = i % size;

            Vector3 location = new Vector3(colNumber - 5, 1f, (i / size) - 5);
            //Debug.Log($"i: {i} location: {location}");
            GameObject block = Instantiate(gridObject.gameObject, location, Quaternion.identity);
            block.transform.SetParent(transform);
        }
    }
}
