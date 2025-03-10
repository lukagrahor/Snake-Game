using System;
using UnityEngine;

public class Food : MonoBehaviour, IPickup
{
    FoodSpawner spawner;
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
}
