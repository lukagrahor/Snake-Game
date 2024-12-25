using UnityEngine;
using UnityEngine.EventSystems;

public class SnakePart : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    bool enableMovement = false;
    Vector3 moveDirection = new Vector3(0, 0, 0);
    float moveSpeed = 0f;
    Vector3 turnPosition;
    Vector3 turnDirection;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    public void PrepareForTurn(Vector3 direction, float speed, Vector3 turnPosition, Vector3 turnDirection)
    {
        transform.SetParent(null);
        moveSpeed = speed;
        moveDirection = direction * speed;
        enableMovement = true;
        Debug.Log("turnPosition" + ": " + turnPosition);
        Debug.Log("transform position" + ": " + transform.position);
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

    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction * moveSpeed;
    }
}
