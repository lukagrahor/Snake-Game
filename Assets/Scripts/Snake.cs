using UnityEngine;
using System.Collections.Generic;

public class Snake : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject snakeHeadPrefab;
    [SerializeField] GameObject snakePartPrefab;
    SnakeMovement SnakeMovement;
    GameObject SnakeHeadObject;
    List<GameObject> SnakeParts;

    Vector3 moveDirection;
    [SerializeField] float moveSpeed = 2f;
    void Awake()
    {
        SnakeParts = new List<GameObject>();
        moveDirection = new Vector3(1f, 0, 0);
        SnakeHeadObject = Instantiate(snakeHeadPrefab);
        SnakeHeadObject.transform.SetParent(transform);
        // arena je na poziciji 0, kocka arene jevelika 1, kar pomeni da gre za 0.5 gor od 0, kocka od kaèe pa je velika 0.5 --> 0.25
        SnakeMovement = gameObject.AddComponent<SnakeMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        Move(moveDirection);
    }

    public void Move(Vector3 direction)
    {
        transform.Translate(direction * Time.deltaTime);
    }

    public Vector3 GetDirection()
    {
        return moveDirection;
    }

    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction * moveSpeed;
    }

    public void Grow()
    {
        Debug.Log("Grow!");
        GameObject newSnakePart = Instantiate(snakePartPrefab);
        //newSnakePart.transform.SetParent(transform);
        SnakeParts.Add(newSnakePart);
    }
}
