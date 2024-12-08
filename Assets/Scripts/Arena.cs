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
        for (int i = 0; i < size*size; i++)
        {
            Vector3 location = new Vector3(i + 0.5f, 0f, i / size);
            Instantiate(ArenaBlock, location, Quaternion.identity);
        }
    }
}
