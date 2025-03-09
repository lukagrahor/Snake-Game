using UnityEngine;

public class GridObject : MonoBehaviour
{
    bool isOccupied = false;
    bool isOccupiedBySnakehead = false;
    int col;
    int row;

    public int Col { get => col; set => col = value; }
    public int Row { get => row; set => row = value; }
    public bool IsOccupied { get => isOccupied; }
    public bool IsOccupiedBySnakehead { get => isOccupiedBySnakehead; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ISnakePart>() != null)
        {
            isOccupied = true;
        }

        if (other.GetComponent<SnakeHead>() != null)
        {
            isOccupiedBySnakehead = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
       if (other.GetComponent<ISnakePart>() != null && other.GetComponent<ISnakePart>().isLast() == true)
       {
            isOccupied = false;
       }

        if (other.GetComponent<SnakeHead>() != null)
        {
            isOccupiedBySnakehead = false;
        }
    }
}
