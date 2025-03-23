using UnityEngine;

public class GridObject : MonoBehaviour, ISnakeHeadTriggerHandler
{
    bool isOccupied = false;
    bool isOccupiedBySnakeHead = false;
    int col;
    int row;
    bool canTurn = true;

    public int Col { get => col; set => col = value; }
    public int Row { get => row; set => row = value; }
    public bool IsOccupied { get => isOccupied; set => isOccupied = value; }
    public bool IsOccupiedBySnakeHead { get => isOccupiedBySnakeHead; set => isOccupiedBySnakeHead = value; }

    public void HandleTrigger(SnakeHead snakeHead)
    {
        snakeHead.SetHasSnapped(false);
        snakeHead.SetNextBlock(this);
        isOccupiedBySnakeHead = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        IGridObjectTriggerHandler enteredObject = other.GetComponent<ISnakePart>();
        enteredObject?.HandleTrigger(this);
    }

    private void OnTriggerExit(Collider other)
    {
        IGridObjectTriggerHandler enteredObject = other.GetComponent<ISnakePart>();
        enteredObject?.HandleTriggerExit(this);
    }

    //public bool allowTurn() { return canTurn; }
}
