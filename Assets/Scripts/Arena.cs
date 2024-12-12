using UnityEngine;

public class Arena : MonoBehaviour
{
    [SerializeField] GameObject ArenaBlock;
    [SerializeField] int size = 10;
    void Start()
    {
        SpawnArena();
    }

    void Update()
    {
        
    }

    void SpawnArena()
    {
        int colNumber = 0;
        for (int i = 0; i < size * size; i++)
        {
            colNumber = i % size;
            Vector3 location = new Vector3(colNumber - 5, 0f, (i / size) - 5);
            GameObject block = Instantiate(ArenaBlock, location, Quaternion.identity);
            block.transform.SetParent(transform);
        }
    }
}
