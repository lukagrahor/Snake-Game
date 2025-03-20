using System;
using System.Drawing;
using UnityEngine;

public class Food : MonoBehaviour, IPickup, ISnakeHeadTriggerHandler
{
    FoodSpawner spawner;
    [SerializeField] float size = 0.5f;

    public void HandleTrigger(SnakeHead snakeHead)
    {
        snakeHead.Grow();
        Use();
    }

    public void Use() {
        gameObject.SetActive(false);
        spawner.Spawn();
    }

    public void SetSpawner(ObjectSpawner spawner)
    {
        this.spawner = (FoodSpawner)spawner;
    }

    public void SetNewPosition(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
    }

    public void ApplyScale()
    {
        gameObject.transform.localScale = new Vector3(size, size, size);
    }
}
