using UnityEngine;

public class StationaryEnemy : MonoBehaviour, ISpawnableObject, IFrontTriggerHandler
{
    public void GetHit()
    {
        Destroy(gameObject);
    }

    public void HandleFrontTrigger()
    {
        GetHit();
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
