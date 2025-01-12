using System;
using UnityEngine;

public class Food : MonoBehaviour, IPickup
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //[SerializeField] UnityEvent growSnake;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
      
    public void Use(Snake snake) {
        snake.Grow();
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"Collision: {other.GetComponent<Food>()}");
        //Debug.Log("A bo al nje?");
        if (other.GetComponent<SnakeHead>() != null)
        {
            Use(other.GetComponentInParent<Snake>());
        }
    }
}
