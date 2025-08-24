using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameManager gameManager;
    [SerializeField] TMP_Text foodCounter;
    [SerializeField] FoodSpawner foodSpawner;    
    [SerializeField] TMP_Text partCounter;
    [SerializeField] Snake snake;
    void Start()
    {
        FoodActions.Eaten += CollectFood;
        FoodActions.EatenByPlayer += UpdatePartCountOnEat;
        PlayerActions.PlayerHit += UpdatePartCountOnHit;

        UpdatePartCountOnHit();
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

    void UpdatePartCountOnEat() {
        int snakeSize = snake.NewLevelSize;
        int snakeMaxSize = snake.MaxSnakeSize;
        partCounter.text = snakeSize + "/" + snakeMaxSize;
    }

    void UpdatePartCountOnHit()
    {
        int snakeSize = snake.SnakeSize;
        int snakeMaxSize = snake.MaxSnakeSize;
        partCounter.text = snakeSize + "/" + snakeMaxSize;
    }

    private void OnDestroy()
    {
        FoodActions.Eaten -= CollectFood;
        FoodActions.Eaten -= UpdatePartCountOnEat;
        PlayerActions.PlayerHit -= UpdatePartCountOnHit;
    }
}
