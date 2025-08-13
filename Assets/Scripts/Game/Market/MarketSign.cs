using System.Collections.Generic;
using UnityEngine;

public class MarketSign : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Snake snake;
    [SerializeField] GameObject snakeHeadPrefab;
    [SerializeField] GameObject snakeTorsoPrefab;
    List<GameObject> torsoParts;
    Color defaultColor1;
    Color defaultColor2;
    int snakeSize;
    int lineSize = 8;
    bool marksPresent = false;
    private void OnEnable()
    {
        torsoParts = new List<GameObject>();
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
        Debug.Log($"newTorsoModel: {newTorsoModel}");
        newTorsoModel.localRotation = Quaternion.identity;
        newTorsoModel.name = $"partJ{j}";
        torsoParts.Add(newTorsoModel.gameObject);
    }

    void SpawnHead()
    {
        float snakeHeadXSize = 0.0425f;
        GameObject snakeHead = Instantiate(snakeHeadPrefab, transform);
        snakeHead.transform.localScale = new Vector3(snakeHeadXSize, snakeHeadXSize, snakeHeadXSize);
        snakeHead.transform.SetLocalPositionAndRotation(new Vector3(0.15f, -0.038f, 0.07f), Quaternion.Euler(0f, 0f, 180f));
    }

    public void MarkParts(int partCount)
    {
        Debug.Log($"Marking {partCount} parts");
        marksPresent = true;
        for (int i = torsoParts.Count - 1; i >= 0; i--)
        {
            if (partCount == 0) return;
            defaultColor1 = torsoParts[i].gameObject.GetComponent<MeshRenderer>().materials[0].color;
            defaultColor2 = torsoParts[i].gameObject.GetComponent<MeshRenderer>().materials[1].color;
            torsoParts[i].gameObject.GetComponent<MeshRenderer>().materials[0].color = Color.red;
            torsoParts[i].gameObject.GetComponent<MeshRenderer>().materials[1].color = Color.red;
            partCount--;
        }
    }

    public void RemoveMarks(int partCount)
    {
        if (!marksPresent) return;
        marksPresent = false;
        Debug.Log($"Removing marks from {partCount} parts");
        for (int i = torsoParts.Count - 1; i >= 0; i--)
        {
            if (partCount == 0) return;
            torsoParts[i].gameObject.GetComponent<MeshRenderer>().materials[0].color = defaultColor1;
            torsoParts[i].gameObject.GetComponent<MeshRenderer>().materials[1].color = defaultColor2;
            partCount--;
        }
    }
}
