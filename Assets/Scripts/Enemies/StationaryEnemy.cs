using UnityEngine;

public class StationaryEnemy : MonoBehaviour, ISpawnableObject
{
    public void GetHit()
    {
        Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
