using UnityEngine;
using UnityEngine.EventSystems;

public class Snake : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject snakePart;
    SnakeMovement SnakeMovement;

    Vector3 moveDirection;
    [SerializeField] float moveSpeed = 2f;
    void Awake()
    {
        moveDirection = new Vector3(1f, 0, 0);
        GameObject firstPart = Instantiate(snakePart);
        firstPart.transform.SetParent(transform);
        // arena je na poziciji 0, kocka arene jevelika 1, kar pomeni da gre za 0.5 gor od 0, kocka od kaèe pa je velika 0.5 --> 0.25
        firstPart.transform.localPosition = new Vector3(0f, 0.75f, 0f);
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
}
