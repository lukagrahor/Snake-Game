using System;
using UnityEngine;
using UnityEngine.Events;
public class Food : MonoBehaviour, IPickup
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    GameObject foodObject;
    //[SerializeField] UnityEvent growSnake;
    public event Action onUseFood;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use() {
        if (foodObject != null)
        {
            Destroy(foodObject);
        }
        //Debug.Log($"Invoking growSnake on: {growSnake}");
        //growSnake.Invoke();
        if (onUseFood != null)
        {
            onUseFood();
        }
    }
    public void Spawn() {
        foodObject = Instantiate(this.gameObject);
    }

    public void Setup(Snake snake)
    {
        if (snake != null)
        {
            onUseFood += snake.Grow;
            Debug.Log("ey yo");
            //growSnake = new UnityEvent();
            //growSnake.AddListener(snake.Grow);
        }
    }
}
