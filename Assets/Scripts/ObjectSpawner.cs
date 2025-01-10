using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] Food pickup;
    [SerializeField] Snake snake;
    [SerializeField] ArenaBlock arenaBlock;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 food1Position = new Vector3 (0f, arenaBlock.GetBlockSize(), 3f);
        Vector3 food2Position = new Vector3 (3f, arenaBlock.GetBlockSize(), -2f);
        Vector3 food3Position = new Vector3(3f, arenaBlock.GetBlockSize(), 3f);
        Food food1 = Instantiate(pickup, food1Position, Quaternion.identity);
        Food food2 = Instantiate(pickup, food2Position, Quaternion.identity);
        Food food3 = Instantiate(pickup, food3Position, Quaternion.identity);
        //food1.Setup(0f, 3f);
        //food2.Setup(3f, -2f);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
