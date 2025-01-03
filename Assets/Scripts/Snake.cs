using UnityEngine;
using System.Collections.Generic;

public class Snake : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] SnakeHead snakeHeadPrefab;
    [SerializeField] SnakeTorso snakeTorsoPrefab;
    [SerializeField] SnakeCorner snakeCornerPrefab;
    SnakeHead snakeHead;
    List<SnakeTorso> snakeTorsoParts;
    SnakeCorner snakeCorner;

    float snakeYRotation = 0;
    [SerializeField] float moveSpeed = 1f;
    void Awake()
    {
        snakeTorsoParts = new List<SnakeTorso>();
        snakeHead = Instantiate(snakeHeadPrefab.gameObject).GetComponent<SnakeHead>();
        snakeHead.Setup(moveSpeed, snakeYRotation, transform);
        // arena je na poziciji 0, kocka arene je velika 1, kar pomeni da gre za 0.5 gor od 0, kocka od ka�e pa je velika 0.5 --> 0.25
    }

    // Update is called once per frame
    void Update()
    {

    }

    public float GetYRotation()
    {
        return snakeYRotation;
    }

    public void SetYRotation(float turnRotation)
    {
        if (snakeTorsoParts.Count != 0)
        {
            snakeTorsoParts[0].PrepareForTurn(snakeHead.transform.position, turnRotation);

            //snakeCorner = Instantiate(snakeCornerPrefab.gameObject).GetComponent<SnakeCorner>();
            //snakeCorner.Setup(snakeHead.transform);
        }
        snakeHead.SetRotation(turnRotation);
        snakeYRotation = snakeHead.GetRotation();
    }

    public void Grow()
    {
        //Debug.Log("Grow!");
        SnakeTorso newSnakeTorso = Instantiate(snakeTorsoPrefab.gameObject).GetComponent<SnakeTorso>();
        //Debug.Log(newSnakePart.name);
        //Debug.Log($"Parenting to: {transform.name}");
        if(snakeTorsoParts.Count == 0)
        {
            newSnakeTorso.transform.SetParent(snakeHead.transform);
            snakeHead.unsetLast();
        }
        else
        {
            snakeTorsoParts[snakeTorsoParts.Count].unsetLast();
        }
        newSnakeTorso.Setup(moveSpeed, snakeYRotation, transform);
        snakeTorsoParts.Add(newSnakeTorso);
    }

    public float GetAbsoluteRotation()
    {
        // get rid of minuses and numbers bigger than 360
        float absoluteMoveRotation = snakeYRotation % 360;
        if (absoluteMoveRotation < 0)
        {
            absoluteMoveRotation = 360 + absoluteMoveRotation;
        }
        return absoluteMoveRotation;
    }
}
