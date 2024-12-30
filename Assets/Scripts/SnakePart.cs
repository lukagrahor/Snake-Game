using UnityEngine;
using UnityEngine.EventSystems;

public class SnakePart : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float moveSpeed = 0f;
    float turnRotation;
    Vector3 turnPosition;
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    public void PrepareForTurn(Vector3 turnPosition, float turnRotation)
    {
        this.turnPosition = turnPosition;
        this.turnRotation = turnRotation;
        moveSpeed += 0.02f;
        //Debug.Log($"turnRotation: {turnRotation}");
    }
    void Move()
    {
        // if the head turned
        if (turnPosition.x != 0 || turnPosition.z != 0)
        {
            CheckAllAxis(transform.rotation.eulerAngles.y);
        }

        //Debug.Log($"moveRotation: {transform.rotation.eulerAngles.y}");
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.forward);
        
    }

    void CheckAllAxis(float moveRotation)
    {
        float absoluteMoveRotation = GetAbsoluteRotation(moveRotation);
        float roundedRotation = Mathf.Round(absoluteMoveRotation / 90f) * 90f;
        if (roundedRotation < 0)
        {
            roundedRotation = 360 + roundedRotation;
        }

        // check if a turn happened on the movement axis
        if (roundedRotation == 0)
        {
            CheckForTurn(transform.position.z, turnPosition.z);
        }

        else if (roundedRotation == 180)
        {
            CheckForTurn(-transform.position.z, -turnPosition.z);
        }

        if (roundedRotation == 90)
        {
            CheckForTurn(transform.position.x, turnPosition.x);
        }

        else if (roundedRotation == 270)
        {
            CheckForTurn(-transform.position.x, -turnPosition.x);
        }
    }

    void CheckForTurn(float currentPosition, float turnPositionOnRequiredAxis)
    {
        if (currentPosition >= turnPositionOnRequiredAxis)
        {
            //Debug.Log($"currentPosition: {currentPosition}");
            //Debug.Log($"turnPositionOnRequireAxis: {turnPositionOnRequiredAxis}");
            Debug.Log("Obrat");
            SetRotation(turnRotation);

            turnRotation = 0f;
            turnPosition = new Vector3(0f, 0f, 0f);
            moveSpeed -= 0.02f;
        }
    }

    /*
    void CheckForTurnXAxis(float moveRotation)
    {
        if (moveRotation <= 0)
        {
            TurnCheck(-transform.position.x, -turnPosition.x);
        }
        else if (moveRotation >= 0)
        {
            TurnCheck(transform.position.x, turnPosition.x);
        }
    }

    void CheckForTurnZAxis(float moveRotation)
    {
        if (moveRotation <= 0)
        {
            TurnCheck(-transform.position.z, -turnPosition.z);
        }
        else if (moveRotation >= 0)
        {
            TurnCheck(transform.position.z, turnPosition.z);
        }
    }
    */


    public void Setup(float moveSpeed, float moveRotation, Transform snakeTransform)
    {
        this.moveSpeed = moveSpeed;

        transform.localPosition = new Vector3(0, 0, -1f);
        SetStartingRotation(moveRotation);
        
        transform.SetParent(snakeTransform);
    }

    public void SetStartingRotation(float rotation)
    {
        //Debug.Log("SETIRAM ROTACIJU");
        transform.eulerAngles = new Vector3(0f , rotation, 0f);
    }

    public void SetRotation(float rotation)
    {
        //Debug.Log("SETIRAM ROTACIJU2");
        transform.Rotate(0f, rotation, 0f);
    }

    float GetAbsoluteRotation(float rotation)
    {
        // get rid of minuses and numbers bigger than 360
        float absoluteMoveRotation = rotation % 360;
        if (absoluteMoveRotation < 0)
        {
            absoluteMoveRotation = 360 + absoluteMoveRotation;
        }
        return absoluteMoveRotation;
    }
}
