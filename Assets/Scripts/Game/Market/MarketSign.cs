using UnityEngine;

public class MarketSign : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Snake snake;
    [SerializeField] GameObject snakeHeadPrefab;
    [SerializeField] GameObject snakeTorsoPrefab;
    int snakeSize;
    int lineSize = 8;
    private void OnEnable()
    {
        ShowSnakeParts();
    }
    public void ShowSnakeParts()
    {
        snakeSize = snake.NewLevelSize;
        bool isFacingRight = true;
        Debug.Log($"Snake has: {snakeSize} parts, of which {snakeSize - 2} are sellable");
        int i = 0;
        int lineCount = 0;


        SpawnHead();

        while (i < snakeSize)
        {
            i = SpawnHorizontalLine(isFacingRight, i, lineCount);
            if (i >= snakeSize) break;

            lineCount++;
            if (isFacingRight) SpawnPart(lineSize - 1, lineCount);
            else SpawnPart(0, lineCount);
            i++;
            isFacingRight = isFacingRight != true;
            if (i >= snakeSize) break;

            lineCount++;
        }
    }

    int SpawnHorizontalLine(bool isFacingRight, int i, int lineCount)
    {
        if (isFacingRight)
        {
            for (int j = 0; j < lineSize; j++)
            {
                
                // space for the head
                if (i == 0 && j == 0)
                {
                    j++;
                    continue;
                }
                
                if (i >= snakeSize) return i;
                SpawnPart(j, lineCount);
                i++;
            }
        }
        else
        {
            for (int j = lineSize - 1; j >= 0; j--)
            {
                if (i >= snakeSize) return i;
                SpawnPart(j, lineCount);
                i++;
            }
        }
        return i;
    }

    void SpawnPart(int j, int lineCount)
    {
        float snakeTorsoXSize = 0.041f;
        float snakeTorsoZSize = 0.030f;
        GameObject newTorso = Instantiate(snakeTorsoPrefab, transform);
        newTorso.transform.localScale = new Vector3(snakeTorsoXSize, snakeTorsoXSize, snakeTorsoXSize);
        newTorso.transform.SetLocalPositionAndRotation(new Vector3(0.14f - j * snakeTorsoXSize, -0.038f, 0.063f - (lineCount * snakeTorsoZSize)), Quaternion.identity);
        Transform newTorsoModel = newTorso.transform.GetChild(0);
        newTorsoModel.localRotation = Quaternion.identity;
    }

    void SpawnHead()
    {
        float snakeHeadXSize = 0.0425f;
        GameObject snakeHead = Instantiate(snakeHeadPrefab, transform);
        snakeHead.transform.localScale = new Vector3(snakeHeadXSize, snakeHeadXSize, snakeHeadXSize);
        snakeHead.transform.SetLocalPositionAndRotation(new Vector3(0.15f, -0.038f, 0.07f), Quaternion.Euler(0f, 0f, 180f));
    }
}
