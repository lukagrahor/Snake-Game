using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    int foodCollected;
    int maxFoodCount;
    [SerializeField] TMP_Text foodCounter;
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
        maxFoodCount = foodCount;
        foodCollected = 0;
        foodCounter.text = foodCollected + "/" + maxFoodCount;
    }

    void CollectFood()
    {
        foodCollected++;
        foodCounter.text = foodCollected + "/" + maxFoodCount;
        if (foodCollected < maxFoodCount) return;
        // naslednji nivo
    }
}
