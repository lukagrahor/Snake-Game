using UnityEngine;

public class GridObject : MonoBehaviour, ISnakeHeadTriggerHandler, ISnakeHeadExitTriggerHandler, IChaseEnemytrigger, IDogTriggerHandler
{
    bool isOccupied = false;
    bool isOccupiedBySnakeHead = false;
    bool hasPathMarker = false;
    int col;
    int row;

    public int Col { get => col; set => col = value; }
    public int Row { get => row; set => row = value; }
    public bool IsOccupied { get => isOccupied; set => isOccupied = value; }
    public bool IsOccupiedBySnakeHead { get => isOccupiedBySnakeHead; set => isOccupiedBySnakeHead = value; }
    public bool HasPathMarker { get => hasPathMarker; set => hasPathMarker = value; }

    public void HandleTrigger(SnakeHead snakeHead)
    {
        //snakeHead.SetHasSnapped(false);
        //Debug.Log($"Enter: {gameObject.name} {transform.position}");
        snakeHead.SetNextBlock(this);
        isOccupiedBySnakeHead = true;
        HasPathMarker = false;
        //Debug.Log("Ka�a");
    }

    public void HandleChaseEnemyTrigger(ChaseEnemy enemy)
    {
        enemy.NextBlock = this;
        //Debug.Log("Nasprotnik");
    }

    public void HandleSnakeheadTriggerExit(SnakeHead snakeHead)
    {
        //snakeHead.SetHasSnapped(false);
        //Debug.Log($"Exit: {gameObject.name} {transform.position}");
        //Debug.Log("");
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
