using System.Collections.Generic;
using UnityEngine;

public class SnakeHead : MonoBehaviour, ISnakePart, IWaspFrontTriggerHandler
{
    [SerializeField] Directions startingRotation = Directions.Up;
    Canvas abilityChargeCanvas;
    public float MoveSpeed { get; set; }
    bool lastSnakePart = true;

    Snake snake;
    GridObject nextBlock;
    GridObject lastRotationBlock;
    Quaternion biteMoveRotation;
    Vector3 biteMoveDirecton;
    LinkedList<float> rotationBuffer;
    SnakeHeadStateMachine stateMachine;

    //bool stop = false;
    enum Directions
    {
        Up = 0,
        Right = 90,
        Down = 180,
        Left = 270
    }

    public GridObject NextBlock { get => nextBlock; set => nextBlock = value; }
    public GridObject LastRotationBlock { get => lastRotationBlock; set => lastRotationBlock = value; }
    public Snake Snake { get => snake; }
    public LinkedList<float> RotationBuffer { get => rotationBuffer; set => rotationBuffer = value; }
    public SnakeHeadStateMachine StateMachine { get => stateMachine;}
    public Canvas AbilityChargeCanvas { get => abilityChargeCanvas; set => abilityChargeCanvas = value; }

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
        stateMachine = new SnakeHeadStateMachine(this);
        stateMachine.Intialize();
    }

    void OnEnable()
    {
        transform.rotation = Quaternion.Euler(0, (float)startingRotation, 0);
    }

    void Update()
    {
        /*
        if (stop == true)
        {
            return;
        }
        */
        stateMachine.Update();
        /*
        if (isBiting) MoveWhileBiting();
        else Move();
        */
    }
    /*
    public void Stop()
    {
        stop = true;
    }
    */

    public void Setup(float moveSpeed, Snake snake, Vector3 scale)
    {
        this.snake = snake;
        transform.SetParent(snake.transform);
        transform.localScale = scale;
        this.MoveSpeed = moveSpeed;
        if (abilityChargeCanvas != null)
        {
            Transform canvasTransform = abilityChargeCanvas.transform;
            canvasTransform.SetParent(transform);
            canvasTransform.localPosition = new Vector3(0f, 0f, canvasTransform.localPosition.z);
            GameObject arrow = canvasTransform.Find("Arrow").gameObject;
            RectTransform ArrowTransform = arrow.GetComponent<RectTransform>();
            ArrowTransform.localPosition = new Vector3(0f, 0f, -0.748f);
            ArrowTransform.sizeDelta = new Vector2(2f, 0.1f);
            //arrow.local = new Vector3(0, 8f, 0);
        }
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
        if (stateMachine.CurrentState == stateMachine.NormalState)
        {
            Debug.Log("NormalState");
            if (lastRotationBlock != null && other.gameObject.name == lastRotationBlock.gameObject.name)
            {
                return;
            }
            if (other.GetComponent<GridObject>() != null)
            {
                SnakeNormalState normalState = (SnakeNormalState)stateMachine.CurrentState;
                normalState.OnGridBlockStay(other);
            }
        }
    }

    public void Move()
    {
        // Vector3.forward --> local space, transform.forward --> world space
        transform.Translate(MoveSpeed * Time.deltaTime * Vector3.forward);
    }
    // neodvisno od rotacije glave --> glava se lahko rotira, ampak ka�a potuje naprej

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

    public void GetHit()
    {
        snake.GetHit();
    }

    public void Grow()
    {
        snake.Grow();
    }
    /*
    Vector3 RotationToMovementVector(float rotation)
    {
        // rotacije niso zmeraj tako kot bi si �elel
        // 90.000001 --> pri rotaciji pride do float precision errors, zato zaokor�im
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
    */
    public Snake GetSnake()
    {
        return snake;
    }

    public void HandleTrigger(Wasp wasp)
    {
        WaspStateMachine stateMachine = wasp.Ai.waspStateMachine;
        if (stateMachine.CurrentState == stateMachine.ChargeState)
        {
            stateMachine.ChargeState.CoolDown();
        }
        snake.GetHit();
    }

    /*
    public void SetBiteMovementDirection()
    {
        biteMoveRotation = Quaternion.Euler(0f, GetRotation(), 0f);
        biteMoveDirecton = RotationToMovementVector(GetRotation());
    }*/

    public void StartBiting()
    {
        stateMachine.TransitionTo(stateMachine.BitingState);
    }

    public void StopBiting()
    {
        stateMachine.TransitionTo(stateMachine.NormalState);
    }
}
