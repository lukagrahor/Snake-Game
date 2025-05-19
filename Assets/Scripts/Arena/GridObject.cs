using UnityEngine;

public class GridObject : MonoBehaviour, ISnakeHeadTriggerHandler, ISnakeHeadExitTriggerHandler, IChaseEnemytrigger, IDogTriggerHandler
{
    bool isOccupied = false;
    bool isOccupiedBySnakeHead = false;
    bool hasPathMarker = false;
    int col;
    int row;
    SnakePathMarker marker;
    Food food;

    public int Col { get => col; set => col = value; }
    public int Row { get => row; set => row = value; }
    public bool IsOccupied { get => isOccupied; set => isOccupied = value; }
    public bool IsOccupiedBySnakeHead { get => isOccupiedBySnakeHead; set => isOccupiedBySnakeHead = value; }
    public bool HasPathMarker { get => hasPathMarker; set => hasPathMarker = value; }
    public SnakePathMarker Marker { get => marker; set => marker = value; }
    public Food Food { get => food; set => food = value; }

    public void HandleTrigger(SnakeHead snakeHead)
    {
        snakeHead.NextBlock = this;
        isOccupiedBySnakeHead = true;
        HasPathMarker = false;
    }

    public void HandleChaseEnemyTrigger(Fly enemy)
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

    public void HandleTrigger(DogPatrolState patrolState, Dog dog)
    {
        dog.NextBlock = this;
    }
}
