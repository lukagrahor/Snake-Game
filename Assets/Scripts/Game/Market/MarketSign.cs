using UnityEngine;

public class MarketSign : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Snake snake;
    int snakeSize;
    private void OnEnable()
    {
        ShowSnakeParts();
    }
    public void ShowSnakeParts()
    {
        snakeSize = snake.NewLevelSize;
        Debug.Log($"Snake has: {snakeSize} parts, of which {snakeSize - 2} are sellable");
    }
}
