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

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    public void PrepareForTurn(Vector3 turnPosition, Vector3 turnDirection)
    {
        this.turnPosition = turnPosition;
        this.turnDirection = turnDirection;
        Debug.Log($"turnDirection: {turnDirection}");
    }
    void Move()
    {
        if (turnPosition.x != 0 || turnPosition.z != 0)
        {
            if (moveDirection.x != 0)
            {
                CheckForTurnXAxis(moveDirection.x);
            }
            else if (moveDirection.z != 0)
            {
                CheckForTurnZAxis(moveDirection.z);
            }
        }
        /*
        if (transform.position.x >= turnPosition.x)
        {
            moveDirection = turnDirection * moveSpeed;
        }
        */
        Debug.Log($"moveDirection: {this.moveDirection}");
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    void CheckForTurnXAxis(float axisDirection)
    {
        if (axisDirection <= 0)
        {
            TurnCheck(-transform.position.x, -turnPosition.x);
        }
        else if (axisDirection >= 0)
        {
            TurnCheck(transform.position.x, turnPosition.x);
        }
    }

    void CheckForTurnZAxis(float axisDirection)
    {
        if (axisDirection <= 0)
        {
            TurnCheck(-transform.position.z, -turnPosition.z);
        }
        else if (axisDirection >= 0)
        {
            TurnCheck(transform.position.z, turnPosition.z);
        }
    }

    void TurnCheck(float currentPosition, float turnPositionOnRequireAxis)
    {
        if (currentPosition >= turnPositionOnRequireAxis)
        {
            Debug.Log($"currentPosition: {currentPosition}");
            Debug.Log($"turnPositionOnRequireAxis: {turnPositionOnRequireAxis}");
            moveDirection = turnDirection;
        }
    }

    public void Setup(float moveSpeed, Vector3 moveDirection, Transform snakeTransform)
    {
        this.moveSpeed = moveSpeed;
        this.moveDirection = moveDirection;
        transform.localPosition = new Vector3(-0.5f, 0, 0);
        transform.SetParent(snakeTransform);
    }

    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction;
    }
}
