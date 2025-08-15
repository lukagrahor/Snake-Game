using System.Collections.Generic;
using UnityEngine;
using GlobalEnums;

public class SnakeHead : MonoBehaviour, ISnakePart, IWaspFrontTriggerHandler
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] LayerMask layersToHit;
    [SerializeField] MeshRenderer headRenderer;
    [SerializeField] SnakeHeadFront front;
    Canvas abilityChargeCanvas;
    Directions startingDirection = Directions.Up;
    public float MoveSpeed { get; set; }
    bool lastSnakePart = true;
    bool biteCancelled = true;

    Snake snake;
    GridObject nextBlock;
    GridObject lastRotationBlock;
    Quaternion biteMoveRotation;
    Vector3 biteMoveDirecton;
    LinkedList<float> rotationBuffer;
    SnakeHeadStateMachine stateMachine;
    int growthMultiplier = 1;

    //bool stop = false;

    public GridObject NextBlock { get => nextBlock; set => nextBlock = value; }
    public GridObject LastRotationBlock { get => lastRotationBlock; set => lastRotationBlock = value; }
    public Snake Snake { get => snake; }
    public LinkedList<float> RotationBuffer { get => rotationBuffer; set => rotationBuffer = value; }
    public SnakeHeadStateMachine StateMachine { get => stateMachine; }
    public Canvas AbilityChargeCanvas { get => abilityChargeCanvas; set => abilityChargeCanvas = value; }
    public GameObject Arrow { get; set; }
    public LineRenderer LineRenderer { get => lineRenderer; set => lineRenderer = value; }
    public bool BiteCancelled { get => biteCancelled; set => biteCancelled = value; }
    public SnakeHeadFront Front { get => front; } 
    public int GrowthMultiplier { get => growthMultiplier; set => growthMultiplier = value; }

    public void HandleTrigger(GridObject gridObject)
    {
        gridObject.IsOccupied = true;
        gridObject.SetMarker();
    }

    public void HandleTriggerExit(GridObject gridObject)
    {
        gridObject.IsOccupied = false;
        gridObject.IsOccupiedBySnakeHead = false;
        startingDirection = snake.StartingDirection;
        //gridObject.RemoveMarker();
    }

    void Awake()
    {
        lineRenderer.useWorldSpace = true;
        lineRenderer.enabled = false;
        rotationBuffer = new LinkedList<float>();
        stateMachine = new SnakeHeadStateMachine(this, layersToHit);
    }

    void OnEnable()
    {
        transform.rotation = Quaternion.Euler(0, (float)startingDirection, 0);
        if (stateMachine == null) stateMachine = new SnakeHeadStateMachine(this, layersToHit);
        if (stateMachine.CurrentState != null && stateMachine.CurrentState != stateMachine.SpawnedState)
        {
            stateMachine.TransitionTo(stateMachine.SpawnedState);
        }
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
            canvasTransform.localPosition = new Vector3(0f, 0f, 14f);
            Arrow = canvasTransform.Find("Arrow").gameObject;
            RectTransform ArrowTransform = Arrow.GetComponent<RectTransform>();
            ArrowTransform.localPosition = new Vector3(0f, -0.02f, -0.627f);
            ArrowTransform.sizeDelta = new Vector2(2f, 0.1f);
        }
    }

    public void SetStateMachine()
    {
        stateMachine.Intialize();
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
        if (stateMachine.CurrentState != stateMachine.PowerState)
        {
            if (lastRotationBlock != null && other.gameObject.name == lastRotationBlock.gameObject.name)
            {
                return;
            }
            if (other.GetComponent<GridObject>() != null)
            {
                IRegularMovement normalState = (IRegularMovement)stateMachine.CurrentState;
                normalState.OnGridBlockStay(other);
            }
        }
    }

    public void Move()
    {
        // Vector3.forward --> local space, transform.forward --> world space
        transform.Translate(MoveSpeed * Time.deltaTime * Vector3.forward);
    }
    // neodvisno od rotacije glave --> glava se lahko rotira, ampak kaèa potuje naprej

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
        if (stateMachine.CurrentState == stateMachine.SpawnedState) return;
        snake.GetHit();
    }

    public void HitWall()
    {
        snake.HitWall();
    }

    public void Grow()
    {
        for (int i = 0; i < growthMultiplier; i++)
        {
            snake.Grow();
        }
    }
    /*
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
        if (biteCancelled == true)
        {
            stateMachine.TransitionTo(stateMachine.PowerState);
            biteCancelled = false;
        }
    }

    public void StopBiting()
    {
        biteCancelled = true;
        if (stateMachine.CurrentState != stateMachine.PowerState) return;
        stateMachine.TransitionTo(stateMachine.NormalState);
    }

    public void SetToTransparent()
    {
        Color tempColor = headRenderer.sharedMaterial.color;
        tempColor.a = 0.7f;
        headRenderer.sharedMaterial.color = tempColor;
        snake.SetToTransparent();
    }

    public void SetToSolid()
    {
        Color tempColor = headRenderer.sharedMaterial.color;
        tempColor.a = 1f;
        headRenderer.sharedMaterial.color = tempColor;
        snake.SetToSolid();
    }
}
