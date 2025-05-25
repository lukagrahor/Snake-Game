using UnityEngine;

public class Bee : Enemy, ISpawnableObject, IWaspFrontTriggerHandler
{ 
    [SerializeField][Range(0, 7)] float moveSpeed = 2f;
    enum Directions
    {
        Up = 0,
        Right = 90,
        Down = 180,
        Left = 270
    }

    public override void Setup(int col, int row, int gridSize)
    {
        if (row == 0 && col == 0)
        {
            int random = Random.Range(1, 3);
            if (random == 1)
            {
                transform.Rotate(0f, (float)Directions.Right, 0f);
            }
        }
        else if (row == (gridSize - 1) && col == (gridSize - 1))
        {
            int random = Random.Range(1, 3);
            if (random == 1)
            {
                transform.Rotate(0f, (float)Directions.Down, 0f);
            }
            else
            {
                transform.Rotate(0f, (float)Directions.Left, 0f);
            }
        }
        else if (col == 0)
        {
            transform.Rotate(0f, (float)Directions.Right, 0f);
        }
        else if (col == (gridSize - 1))
        {
            transform.Rotate(0f, (float)Directions.Left, 0f);
        }
        else if (row == (gridSize - 1))
        {
            transform.Rotate(0f, (float)Directions.Down, 0f);
        }
    }

    protected override void GetHit()
    {
        Destroy(gameObject);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void Turn()
    {
        transform.Rotate(0f, 180f, 0f);
    }

    void Move()
    {
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.forward);
    }

    public void HandleTrigger(Wasp wasp)
    {
        WaspStateMachine stateMachine = wasp.Ai.waspStateMachine;
        if (stateMachine.CurrentState == stateMachine.ChargeState)
        {
            stateMachine.ChargeState.CoolDown();
        }
        GetHit();
    }
}
