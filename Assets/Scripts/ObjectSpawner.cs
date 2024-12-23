using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject pickup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pickup.GetComponent<Food>().Spawn();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
