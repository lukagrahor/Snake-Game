using System;
using UnityEngine;

public class Food : MonoBehaviour, IPickup
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //[SerializeField] UnityEvent growSnake;
    ObjectSpawner spawner;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
      
    public void Use(Snake snake) {
        snake.Grow();
        spawner.SpawnFood();
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<SnakeHead>() != null)
        {
            Use(other.GetComponentInParent<Snake>());
        }
    }

    public void SetObjectSpawner(ObjectSpawner spawner)
    {
        this.spawner = spawner;
    }
}
