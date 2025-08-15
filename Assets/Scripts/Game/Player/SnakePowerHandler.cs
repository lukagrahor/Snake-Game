using UnityEngine;

public class SnakePowerHandler : MonoBehaviour
{
    // This class oversees the snake's powers
    // active and passive abilities
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Snake snake;
    SnakeHead snakeHead;
    bool doubleGrowthActive = false;
    public bool DoubleGrowthActive { get => doubleGrowthActive; set => doubleGrowthActive = value; }

    void Start()
    {
        snakeHead = snake.SnakeHead;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPower(Power newPower)
    {
        Debug.Log($"Power added: {newPower.name}");
        newPower.EnablePower(snake);
    }
}
