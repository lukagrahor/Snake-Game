using UnityEngine;

public class GridObject : MonoBehaviour, ISnakeHeadTriggerHandler, ISnakeHeadExitTriggerHandler, IFlytrigger, IDogTriggerHandler
{
    bool isOccupied = false;
    bool isOccupiedBySnakeHead = false;
    bool isOccupiedByWall = false;
    bool hasPathMarker = false;
    int col;
    int row;
    SnakePathMarker marker;
    Food food;
    [SerializeField] GameObject occupiedMarkerPrefab;
    //GameObject occupiedMarker;
    float headResetTime = 3f;
    CountDown timer;

    public int Col { get => col; set => col = value; }
    public int Row { get => row; set => row = value; }
    public bool IsOccupied { get => isOccupied; set => isOccupied = value; }
    public bool IsOccupiedBySnakeHead { get => isOccupiedBySnakeHead; set => isOccupiedBySnakeHead = value; }
    public bool IsOccupiedByWall { get => isOccupiedByWall; set => isOccupiedByWall = value; }
    public bool HasPathMarker { get => hasPathMarker; set => hasPathMarker = value; }
    public SnakePathMarker Marker { get => marker; set => marker = value; }
    public Food Food { get => food; set => food = value; }

    public void Update()
    {
        timer?.Update();
    }

    public void HandleTrigger(SnakeHead snakeHead)
    {
        snakeHead.NextBlock = this;
        StartHeadTimer();
        isOccupiedBySnakeHead = true;
        HasPathMarker = false;
    }

    public void HandleFlyTrigger(Fly enemy)
    {
        if (!enemy.IsRotating) enemy.NextBlock = this;
    }

    public void HandleSnakeheadTriggerExit(SnakeHead snakeHead)
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        IGridObjectTriggerHandler enteredObject = other.GetComponent<ISnakePart>();
        enteredObject?.HandleTrigger(this);
    }

    private void OnTriggerStay(Collider other)
    {
        IGridObjectStayTriggerHandler enteredObject = other.GetComponent<IGridObjectStayTriggerHandler>();
        enteredObject?.HandleStayTrigger(this);
    }

    private void OnTriggerExit(Collider other)
    {
        IGridObjectTriggerHandler enteredObject = other.GetComponent<ISnakePart>();
        enteredObject?.HandleTriggerExit(this);
    }

    public void HandleTrigger(IState patrolState, Dog dog)
    {
        dog.NextBlock = this;
    }
    /*
    public void SetMarker()
    {
        if (occupiedMarker == null)
        {
            occupiedMarker = Instantiate(occupiedMarkerPrefab, transform.position, Quaternion.identity);
            occupiedMarker.transform.SetParent(transform);
        }
    }

    public void RemoveMarker()
    {
        if (occupiedMarker != null)
        {
            Destroy(occupiedMarker);
        }
    }
    */

    void StartHeadTimer()
    {
        timer = new CountDown(headResetTime);
        timer.Start();
        timer.TimeRanOut += ResetOccupation;
    }

    void ResetOccupation()
    {
        isOccupied = false;
        isOccupiedBySnakeHead = false;
        //Destroy(occupiedMarker);
    }

    public void StopHeadTimer()
    {
        timer?.Stop();
    }


}
