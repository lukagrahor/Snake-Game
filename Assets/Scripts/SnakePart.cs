using UnityEngine;
using UnityEngine.EventSystems;

public class SnakePart : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    bool enableMovement;
    float moveSpeed = 0f;
    Vector3 moveDirection = new Vector3(0, 0, 0);
    Vector3 turnPosition;
    Vector3 turnDirection;
    void Start()
    {
        enableMovement = false;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    public void PrepareForTurn(Vector3 turnPosition, Vector3 turnDirection)
    {
        transform.SetParent(null);
        enableMovement = true;
        this.turnPosition = turnPosition;
        this.turnDirection = turnDirection;
    }
    void Move()
    {
        if (enableMovement)
        {
            if (transform.position.x >= turnPosition.x)
            {
                moveDirection = turnDirection * moveSpeed;
            }
            transform.Translate(moveDirection * Time.deltaTime);
        }
    }

    public void Setup(float moveSpeed, Vector3 moveDirection)
    {
        this.moveSpeed = moveSpeed;
        this.moveDirection = moveDirection;
        transform.localPosition = new Vector3(-0.5f, 0, 0);
    }

    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction * moveSpeed;
    }
}
