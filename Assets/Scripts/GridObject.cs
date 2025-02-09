using UnityEngine;

public class GridObject : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    bool occupied = false;
    bool occupiedBySnakehead = false;
    // for debugging
    int id;
    //pozicija
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
            //Debug.Log($"not occupied: {occupied} {other}");
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

    public void setId(int id)
    {
        this.id = id;
    }

    public int getId()
    {
        return id;
    }
}
