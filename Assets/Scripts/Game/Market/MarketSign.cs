using UnityEngine;

public class MarketSign : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Snake snake;
    [SerializeField] SnakeHead snakeHeadPrefab;
    [SerializeField] SnakeTorso snakeTorsoPrefab;
    int snakeSize;
    private void OnEnable()
    {
        ShowSnakeParts();
    }
    public void ShowSnakeParts()
    {
        snakeSize = snake.NewLevelSize;
        Debug.Log($"Snake has: {snakeSize} parts, of which {snakeSize - 2} are sellable");
        for (int i = 0; i < 8; i++)
        {
            float snakeTorsoSize = 0.041f;
            SnakeTorso newTorso = Instantiate(snakeTorsoPrefab, transform);
            newTorso.transform.localScale = new Vector3 (snakeTorsoSize, snakeTorsoSize, snakeTorsoSize);
            newTorso.transform.SetLocalPositionAndRotation(new Vector3(0.14f - i * snakeTorsoSize, -0.038f, 0.05f), Quaternion.identity);
            Transform newTorsoModel = newTorso.transform.GetChild(0);
            newTorsoModel.localRotation = Quaternion.identity;
        }
    }
}
