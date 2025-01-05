using System;
using UnityEngine;
using UnityEngine.Events;
public class Food : MonoBehaviour, IPickup
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    GameObject foodObject;
    //[SerializeField] UnityEvent growSnake;
    public event Action onUseFood;
    [SerializeField] ArenaBlock arenaBlock;
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
        foodObject.transform.localPosition = new Vector3(0, arenaBlock.GetBlockSize(), 3f);
        foodObject = Instantiate(this.gameObject);
        foodObject.transform.localPosition = new Vector3(3, arenaBlock.GetBlockSize(), -2f);
    }

    public void Setup(Snake snake)
    {
        if (snake != null)
        {
            onUseFood += snake.Grow;
            //Debug.Log("ey yo");
            //growSnake = new UnityEvent();
            //growSnake.AddListener(snake.Grow);
        }
    }

    Vector3 GetNewPosition()
    {
        return new Vector3(3f, arenaBlock.GetBlockSize(), -2f);
    }
}
