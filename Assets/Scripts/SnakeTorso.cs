using System;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTorso : MonoBehaviour, ISnakePart
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float moveSpeed = 0f;
    float size = 0.4f;
    //float turnRotation;
    //Vector3 turnPosition;
    bool lastSnakePart = true;

    LinkedList<float> rotationBuffer;
    LinkedList<Vector3> positionBuffer;

    private bool hasSnapped = false;
    ISnakePart previousPart;
    float time = 0f;
    bool wait = false;
    void Awake()
    {
        rotationBuffer = new LinkedList<float>();
        positionBuffer = new LinkedList<Vector3>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //showPositions();
        
        Move();
        //Debug.Log($"Pozicija tega dela: {transform.position}");
        //Debug.Log($"Pozicija predhodnika: {previousPart.getTransform().position}");
        //float distanceToPrevious = Vector3.Distance(transform.position, previousPart.getTransform().position);
        //Debug.Log($"Razdalja od predhodnika: {distanceToPrevious}");
    }

    public void AddToRotationBuffer(float rotation)
    {
        //Debug.Log($"Dodaj v buffer: {rotation}");
        rotationBuffer.AddLast(rotation);
    }

    public void AddToPositionBuffer(Vector3 position)
    {
        //Debug.Log($"Dodaj v buffer: {position}");
        positionBuffer.AddLast(position);
        //moveSpeed += 0.03f;
    }
    /*
    public void PrepareForTurn(Vector3 turnPosition, float turnRotation)
    {
        this.turnPosition = new Vector3(Mathf.Round(turnPosition.x * 100) / 100f, 0f, Mathf.Round(turnPosition.z * 100) / 100f);
        this.turnRotation = turnRotation;
        moveSpeed += 0.02f;
        //Debug.Log($"turnRotation: {turnRotation}");
    }
    */
    public void Move()
    {
        // if the head turned
        /*
        if (turnPosition.x != 0 || turnPosition.z != 0)
        {
            CheckAllAxis(transform.rotation.eulerAngles.y);
        }*/

        //Debug.Log($"moveRotation: {transform.rotation.eulerAngles.y}");
        // preveri ali je kocka pred predhodnikom ali za njim
        // �e je pred, po�akej
        // �e je za, pospe�i
        //Debug.Log($"[MOVE] First: {transform.position}");
        /*if (waitForTurn() == true)
        {
            return;
        }*/
        CheckForTurn();
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.forward);
    }
    /*
    bool waitForTurn()
    {
        if (wait == true)
        {
            float distanceToPrevious = Vector3.Distance(transform.position, previousPart.getTransform().position);
            //Debug.Log($"distance to previous part: {distanceToPrevious}");
            if (distanceToPrevious <= size)
            {
                //Debug.Log($"Ratalu mi je :DDD: {distanceToPrevious}");
                return true;
            }
            wait = false;
        }
        return false;
    }
    */
    /*
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
            currentPosition = turnPositionOnRequiredAxis;
            //Debug.Log($"currentPosition: {currentPosition}");
            //Debug.Log($"turnPositionOnRequireAxis: {turnPositionOnRequiredAxis}");
            //Debug.Log("Obrat");
            SetRotation(turnRotation);

            turnRotation = 0f;
            turnPosition = new Vector3(0f, 0f, 0f);
            moveSpeed -= 0.02f;
        }
    }
    */

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
        // dobi moveRotacijo od kocke po obratu --> dobim move rotation 0, moglu bi pa bet 90
        this.moveSpeed = moveSpeed;

        transform.localPosition = new Vector3(0, 0, -1f);
        //Debug.Log($"moveRotation: {moveRotation}");
        SetStartingRotation(moveRotation);

        transform.SetParent(snakeTransform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GridObject>() != null)
        {
            time = Time.realtimeSinceStartup;
            hasSnapped = false;
        }
    }
    /*
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log($"Collision: {other.GetComponent<Food>()}");
        if (other.GetComponent<GridObject>() != null)
        {
            //Debug.Log($"Ka�a glava pozicija {transform.position}");
            // ignore the y axis
            Vector3 gridBlockPosition = new Vector3(other.transform.position.x, 0f, other.transform.position.z);
            Vector3 snakeTorsoPosition = new Vector3(transform.position.x, 0f, transform.position.z);

            //Debug.Log($"Distance: {Vector3.Distance(snakeTorsoPosition, gridBlockPosition)}");

            if (Vector3.Distance(snakeTorsoPosition, gridBlockPosition) <= 0.05f && hasSnapped == false)
            {
                Debug.Log("Jabadabadu1");
                if (rotationBuffer.Count > 0)
                {
                    Debug.Log("Dubidubiduba");
                    transform.position = new Vector3(gridBlockPosition.x, transform.position.y, gridBlockPosition.z);
                    hasSnapped = true;
                    SetRotation();
                }
            }
        }
    }
    */

    private void CheckForTurn()
    {
        //Debug.Log($"Collision: {other.GetComponent<Food>()}");

        //Debug.Log($"Ka�a glava pozicija {transform.position}");
        // ignore the y axis
        if (positionBuffer.Count == 0 || hasSnapped == true)
        {
            return;
        }
        Vector3 blockPositionWithY = positionBuffer.First.Value;
        //Debug.Log($"blockPositionWithY: {blockPositionWithY}");

        Vector3 gridBlockPosition = new Vector3(blockPositionWithY.x, 0f, blockPositionWithY.z);
        Vector3 snakeTorsoPosition = new Vector3(transform.position.x, 0f, transform.position.z);

        //Debug.Log($"Distance: {Vector3.Distance(snakeTorsoPosition, gridBlockPosition)}");
        float floatOffset = Vector3.Distance(snakeTorsoPosition, gridBlockPosition);
        
        //Debug.Log($"Torso {gameObject.name} floatOffset: {floatOffset}");
        //Debug.Log($"Torso {gameObject.name} hasSnapped: {hasSnapped}");
        

        Vector3 movementDirection = RotationToMovementVector(GetRotation());
        Vector3 directionToBlock = gridBlockPosition - transform.position;
        //Debug.Log($"Torso {gameObject.name} movementDirection: {movementDirection}");
        //Debug.Log($"Torso {gameObject.name} directionToBlock: {directionToBlock}");
        float dotProduct = Vector3.Dot(movementDirection, directionToBlock.normalized); // neki je narwbe
        //Debug.Log($"Torso {gameObject.name} dotProdukt: {dotProduct}");
        
        
        if (floatOffset <= 0.03f || dotProduct < 0) // dot product nam pove ali vektorja ka�eta v isto ali nasprotno smer
        {
            //Debug.Log($"aha torso {gameObject.name}");
            if (rotationBuffer.Count > 0)
            {
                //Debug.Log($"Rotation is set {rotationBuffer.First.Value}");
                /*
                Debug.Log("------------------------------------------------");
                Debug.Log($"Torso {gameObject.name}, turn position: {gridBlockPosition}");
                Debug.Log("------------------------------------------------");
                */
                SetRotation();
                //Debug.Log($"roatatacija: {transform.rotation.eulerAngles.y}");
                // na sredino grid kocke
                transform.position = new Vector3(gridBlockPosition.x, transform.position.y, gridBlockPosition.z);
                //snapToPrevious();

                hasSnapped = true;
                //moveSpeed -= 0.03f;
                //Debug.Log($"previousPart: {previousPart}");
            }
        }
        
    }

    public void SetStartingRotation(float rotation)
    {
        //Debug.Log("SETIRAM ROTACIJU");
        transform.eulerAngles = new Vector3(0f, rotation, 0f);
    }

   public void SetRotation()
   {
       //Debug.Log("SETIRAM ROTACIJU2");
       //transform.Rotate(0f, rotation, 0f);
       //Debug.Log("Buraziru desu");
       if (rotationBuffer.Count <= 0)
       {
            return;
       }
        //showPositions();
        //Debug.Log("jadransko morje");
        //Debug.Log($"Torso: {gameObject.name} Rotacija pred novo rotacijo: {GetRotation()}");
        //Debug.Log($"Torso: {gameObject.name} Prva rotacija v torso rotation bufferju: {rotationBuffer.First.Value}");
       //transform.Rotate(0, rotationBuffer.First.Value, 0);
        transform.rotation = Quaternion.Euler(0, GetRotation() + rotationBuffer.First.Value, 0);
        //Debug.Log($"Torso: {gameObject.name} Rotacija po novi rotaciji: {GetRotation()}");
        time = Time.realtimeSinceStartup - time;
       //Debug.Log($"Torso speed: {moveSpeed}");
       //Debug.Log($"�as potreben, da torso dose�e lokacijo rotacije: {time}");
       rotationBuffer.RemoveFirst();
       positionBuffer.RemoveFirst();

       wait = true;
       
   }
    /*
    void snapToPrevious()
    {
        float distanceToPrevious = Vector3.Distance(transform.position, previousPart.getTransform().position);
        //Debug.Log($"Razdalja od predhodnika: {distanceToPrevious}");

        if (distanceToPrevious == size)
        {
            return;
        }

        Debug.Log("Jupiiii");
        Vector3 beforeSnap = transform.position;
        transform.position = previousPart.getTransform().position - previousPart.getTransform().forward * size;
        Vector3 afterSnap = transform.position;
        distanceToPrevious = Vector3.Distance(transform.position, previousPart.getTransform().position);
        Debug.Log("----------------------------------------------------------------------------------------------------");
        Debug.Log($"Razdalja od predhodnika2: {distanceToPrevious}");
        Debug.Log($"[SNAP] Before: {beforeSnap}, After: {afterSnap}, Expected: {previousPart.getTransform().position - previousPart.getTransform().forward * size}");
        Debug.Log("----------------------------------------------------------------------------------------------------");
    }*/

    public void SetPreviousPart(ISnakePart previousPart)
    {
        this.previousPart = previousPart;
    }
    public float GetRotation()
    {
        return transform.rotation.eulerAngles.y;
    }
    /*
    public float GetAbsoluteRotation(float rotation)
    {
        // get rid of minuses and numbers bigger than 360
        float absoluteMoveRotation = rotation % 360;
        if (absoluteMoveRotation < 0)
        {
            absoluteMoveRotation = 360 + absoluteMoveRotation;
        }
        return absoluteMoveRotation;
    }*/
    public bool isLast()
    {
        return lastSnakePart;
    }

    public void setLast()
    {
        lastSnakePart = true;
    }

    public void unsetLast()
    {
        lastSnakePart = false;
    }

    public Transform getTransform()
    {
        return transform;
    }

    public void stopWaiting()
    {
        wait = false;
    }

    void showPositions()
    {
        /*
        int i = 0;
        if (positionBuffer.Count == 0)
        {
            //Debug.Log("Praznu!");
            return;
        }
        foreach (Vector3 pos in positionBuffer)
        {
            //Debug.Log($"Toro position {i}: {pos}");
            i++;
        }
        */
        int j = 0;
        foreach (float rotat in rotationBuffer)
        {
            Debug.Log($"Torso rotation buffer {j}: {rotat}");
            j++;
        }
    }

    public void CopyBuffers (LinkedList<float> rotationBuffer, LinkedList<Vector3> positionBuffer)
    {
        foreach (var rotation in rotationBuffer)
        {
            this.rotationBuffer.AddLast(rotation);
        }

        foreach (var position in positionBuffer)
        {
            this.positionBuffer.AddLast(position);
        }
    }

    public LinkedList<float> GetRotationBuffer()
    {
        return rotationBuffer;
    }
    public LinkedList<Vector3> GetPositionBuffer()
    {
        return positionBuffer;
    }

    Vector3 RotationToMovementVector(float rotation)
    {
        //Debug.Log($"Torso {gameObject.name}, rotation: {rotation}");
        // rotacije niso zmeraj tako kot bi si �elel
        // 90.000001 --> pri rotaciji pride do float precision errors, zato zaokor�im
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
}
