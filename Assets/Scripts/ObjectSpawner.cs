using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] Food pickup;
    [SerializeField] Snake snake;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pickup.Spawn();

    }
    private void OnEnable()
    {
        pickup.Setup(snake);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
