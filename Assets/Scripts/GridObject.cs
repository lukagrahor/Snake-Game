using UnityEngine;

public class GridObject : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    bool occupied = false;
    bool occupiedBySnakehead = false;
    // for debugging
    int id;
    int col;
    int row;
    //pozicija
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(int id, int col, int row)
    {
        SetId(id);
        SetCol(col);
        SetRow(row);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ISnakePart>() != null)
        {
            occupied = true;
            //Debug.Log($"occupied: {occupied} {other}");
        }

        if (other.GetComponent<SnakeHead>() != null)
        {
            occupiedBySnakehead = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
       if (other.GetComponent<ISnakePart>() != null && other.GetComponent<ISnakePart>().isLast() == true)
       {
            occupied = false;
       }

        if (other.GetComponent<SnakeHead>() != null)
        {
            occupiedBySnakehead = false;
        }
    }

    public bool isOccupied()
    {
        return occupied;
    }

    void SetId(int id)
    {
        this.id = id;
    }
    public int GetCol()
    {
        return col;
    }
    public int GetRow()
    {
        return row;
    }
    void SetCol(int col)
    {
        this.col = col;
    }

    void SetRow(int row)
    {
        this.row = row;
    }

    public int getId()
    {
        return id;
    }

    public bool IsOccupiedBySnakehead()
    {
        return occupiedBySnakehead;
    }
}
