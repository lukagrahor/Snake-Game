using System.Collections.Generic;
using UnityEngine;

public class SnakeHead : MonoBehaviour, ISnakePart
{
    [SerializeField] Directions startingRotation = Directions.Up;

    float moveSpeed = 0f;
    bool lastSnakePart = true;

    Snake snake;
    GridObject nextBlock;
    GridObject alreadyTurned;
    LinkedList<float> rotationBuffer;

    bool stop = false;
    enum Directions
    {
        Up = 0,
        Right = 90,
        Down = 180,
        Left = 270
    }

    public void HandleTrigger(GridObject gridObject)
    {
        gridObject.IsOccupied = true;
    }

    public void HandleTriggerExit(GridObject gridObject)
    {
        gridObject.IsOccupied = false;
        gridObject.IsOccupiedBySnakeHead = false;
    }

    void Awake()
    {
        rotationBuffer = new LinkedList<float>();
    }

    void OnEnable()
    {
        transform.rotation = Quaternion.Euler(0, (float)startingRotation, 0);
    }

    void Update()
    {
        if (stop == true)
        {
            return;
        }
        Move();
    }

    public void Stop()
    {
        stop = true;
    }

    public void Setup(float moveSpeed, Snake snake, Vector3 scale)
    {
        this.snake = snake;
        transform.SetParent(snake.transform);
        transform.localScale = scale;
        this.moveSpeed = moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        var enteredObject = other.GetComponent<ISnakeHeadTriggerHandler>();
        enteredObject?.HandleTrigger(this);
    }

    private void OnTriggerExit(Collider other)
    {
        var enteredObject = other.GetComponent<ISnakeHeadExitTriggerHandler>();
        enteredObject?.HandleSnakeheadTriggerExit(this);
    }

    private void OnTriggerStay(Collider other)
    {
        if (alreadyTurned != null && other.gameObject.name == alreadyTurned.gameObject.name)
        {
            return;
        }
        if (other.GetComponent<GridObject>() != null)
        {
            // ignore the y axis
            Vector3 gridBlockPosition = new (other.transform.position.x, 0f, other.transform.position.z);
            Vector3 nextGridBlockPosition = new (nextBlock.transform.position.x, 0f, nextBlock.transform.position.z);
            Vector3 snakeHeadPosition = new (transform.position.x, 0f, transform.position.z);

            Vector3 movementDirection = RotationToMovementVector(GetRotation());
            Vector3 directionToBlock = nextGridBlockPosition - transform.position;
            float dotProduct = Vector3.Dot(movementDirection, directionToBlock.normalized);
            // too small distance can cause the snake to not turn when needed
            // dot product nam pove ali vektorja kažeta v isto ali nasprotno smer
            // && distance <= 0.1f, brez tega se je kocka obrnila ko je bila že na robu kocke, kar je izgledalo èudno
            float distance = Vector3.Distance(snakeHeadPosition, gridBlockPosition);
            if (distance <= 0.03f || (dotProduct < 0 && distance <= 0.1f))
            {
                //Debug.Log($"dotProduct: {dotProduct}");
                //Debug.Log($"distance: {distance}");
                if (rotationBuffer.Count > 0)
                {
                    // snap to the place of the grid block
                    transform.position = new Vector3(gridBlockPosition.x, transform.position.y, gridBlockPosition.z);

                    SetRotation();
                    alreadyTurned = other.GetComponent<GridObject>();
                }
            }
        }
    }

    public void Move()
    {
        // Vector3.forward --> local space, transform.forward --> world space
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.forward);
    }

    void SetRotation()
    {
        if (rotationBuffer.Count <= 0)
        {
            return;
        }

        //Debug.Log("Turn");

        transform.Rotate(0, rotationBuffer.First.Value, 0);
        snake.SetTorsoRotation(rotationBuffer.First.Value);
        rotationBuffer.RemoveFirst();
    }

    public void AddToRotationBuffer(float rotation)
    {
        // to prevent spam
        if (rotationBuffer.Count == 2)
        {
            return;
        }
        rotationBuffer.AddLast(rotation);
    }

    public float GetRotation()
    {
        return transform.rotation.eulerAngles.y;
    }

    public void UnsetLast()
    {
        lastSnakePart = false;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public LinkedList<float> GetRotationBuffer()
    {
        return rotationBuffer;
    }
    public LinkedList<Vector3> GetPositionBuffer()
    {
        return new LinkedList<Vector3>();
    }

    public float GetNextRotation()
    {
        if (rotationBuffer.Count == 0)
        {
            return 0f;
        }
        return rotationBuffer.First.Value;
    }

    public void SetNextBlock(GridObject nextBlock)
    {
        this.nextBlock = nextBlock;
    }
    public GridObject GetNextBlock()
    {
        return nextBlock;
    }

    public void GetHit()
    {
        snake.GetHit();
    }

    public void Grow()
    {
        snake.Grow();
    }

    Vector3 RotationToMovementVector(float rotation)
    {
        // rotacije niso zmeraj tako kot bi si želel
        // 90.000001 --> pri rotaciji pride do float precision errors, zato zaokoržim
        rotation = Mathf.Round(rotation);
        return rotation switch
        {
            0 => new Vector3(0f, 0f, 1f),
            90 => new Vector3(1f, 0f, 0f),
            180 => new Vector3(0f, 0f, -1f),
            270 => new Vector3(-1f, 0f, 0f),
            _ => new Vector3(0f, 0f, 0f),
        };
    }

    public Snake GetSnake()
    {
        return snake;
    }
}
