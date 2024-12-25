using UnityEngine;
using System.Collections.Generic;

public class Snake : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] SnakeHead snakeHeadPrefab;
    [SerializeField] SnakePart snakePartPrefab;
    [SerializeField] GameObject snakeCornerPrefab;
    SnakeHead snakeHead;
    List<SnakePart> snakeParts;
    GameObject snakeCorner;

    Vector3 moveDirection = new Vector3(1f, 0, 0);
    [SerializeField] float moveSpeed = 1f;
    void Awake()
    {
        snakeParts = new List<SnakePart>();
        snakeHead = Instantiate(snakeHeadPrefab.gameObject).GetComponent<SnakeHead>();
        snakeHead.transform.SetParent(transform);
        snakeHead.transform.localPosition = new Vector3(0, 0.75f, 0);

        snakeHead.SetMoveSpeed(moveSpeed);
        snakeHead.SetDirection(moveDirection);
        // arena je na poziciji 0, kocka arene je velika 1, kar pomeni da gre za 0.5 gor od 0, kocka od kaèe pa je velika 0.5 --> 0.25
    }

    // Update is called once per frame
    void Update()
    {
        //Move(moveDirection);
    }
    /*
    public void Move(Vector3 direction)
    {
        transform.Translate(direction * Time.deltaTime);
    }
    */

    public Vector3 GetDirection()
    {
        return moveDirection;
    }

    public void SetDirection(Vector3 turnDirection)
    {
        if (snakeParts.Count != 0)
        {
            snakeParts[0].PrepareForTurn(moveDirection, moveSpeed, snakeHead.transform.position, turnDirection);

            snakeCorner = Instantiate(snakeCornerPrefab);
            snakeCorner.transform.SetParent(snakeHead.transform);
            snakeCorner.transform.localPosition = new Vector3(0, 0, 0);
            snakeCorner.transform.SetParent(null);
        }
        moveDirection = turnDirection;
        snakeHead.SetDirection(moveDirection);
    }

    public void Grow()
    {
        Debug.Log("Grow!");
        SnakePart newSnakePart = Instantiate(snakePartPrefab.gameObject).GetComponent<SnakePart>();
        Debug.Log(newSnakePart.name);
        Debug.Log($"Parenting to: {transform.name}");
        if(snakeParts.Count == 0)
        {
            newSnakePart.transform.SetParent(snakeHead.transform);
            newSnakePart.transform.localPosition = new Vector3(-1f, 0, 0);
        }
        snakeParts.Add(newSnakePart);
    }
}
