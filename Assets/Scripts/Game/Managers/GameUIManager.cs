using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] TMP_Text foodCounter;
    [SerializeField] GameManager gameManager;
    [SerializeField] FoodSpawner foodSpawner;
    void Start()
    {
        FoodActions.Eaten += CollectFood;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMaxFood(int foodCount)
    {
        foodSpawner.MaxFood = foodCount;
        foodSpawner.FoodCollected = 0;
        foodCounter.text = foodSpawner.FoodCollected + "/" + foodSpawner.MaxFood;
    }

    void CollectFood()
    {
        foodSpawner.FoodCollected++;
        foodCounter.text = foodSpawner.FoodCollected + "/" + foodSpawner.MaxFood;
        if (foodSpawner.FoodCollected < foodSpawner.MaxFood) return;
        foodSpawner.FoodCollected = 0;
        gameManager.MoveCamera();
    }

    private void OnDestroy()
    {
        FoodActions.Eaten -= CollectFood;
    }
}
