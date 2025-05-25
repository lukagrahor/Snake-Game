using UnityEngine;
using System;
using System.IO.Pipes;

public class SnakeBitingState : ISnakeState
{
    SnakeHead snakeHead;
    LayerMask layersToHit;
    SnakeHeadStateMachine stateMachine;
    GameObject arrow;
    float arrowWidth = 2f;
    float minArrowLength = 0.1f;
    float maxArrowLength = 0.8f;
    float currentChargeTime;
    float maxChargeTime = 0.8f;
    float currentCalculatedBiteRange;
    LineRenderer lineRenderer;

    //Quaternion biteMoveRotation;
    Vector3 biteMoveDirecton;
    public SnakeBitingState(SnakeHead snakeHead, LayerMask layersToHit, SnakeHeadStateMachine stateMachine)
    {
        this.snakeHead = snakeHead;
        this.layersToHit = layersToHit;
    }

    public void Enter()
    {
        currentChargeTime = 0f;
        SetBiteMovementDirection();
        arrow = snakeHead.Arrow;
        arrow.GetComponent<RectTransform>().sizeDelta = new Vector2(arrowWidth, minArrowLength);
        lineRenderer = snakeHead.LineRenderer;
    }

    public void Exit()
    {
        PerformBiteLinecast();
        snakeHead.transform.forward = biteMoveDirecton;
    }

    public void Update()
    {
        currentChargeTime += Time.deltaTime;
        currentChargeTime = Mathf.Min(currentChargeTime, maxChargeTime);
        currentCalculatedBiteRange = Mathf.Lerp(minArrowLength, maxArrowLength, currentChargeTime / maxChargeTime);
        MoveWhileBiting();
        UpdateIndicator();
    }

    public void SetBiteMovementDirection()
    {
        //biteMoveRotation = Quaternion.Euler(0f, snakeHead.GetRotation(), 0f);
        biteMoveDirecton = RotationToMovementVector(snakeHead.GetRotation());
    }

    public void MoveWhileBiting()
    {
        // ne sme se takoj prekinit, ker èe ne gre kaèa off the grid --> mogoèe buljše vrnt igralca nazaj na prejšnjo rotacijo
        if (biteMoveDirecton == null) return;
        snakeHead.transform.Translate(snakeHead.MoveSpeed * Time.deltaTime * biteMoveDirecton, Space.World);
    }

    public void SetRotation(float turnRotation)
    {
        float biteMoveRotation = MovementVectorToRotation(biteMoveDirecton);
        float currentRotation = snakeHead.GetRotation();
        //Debug.Log("biteMoveRotation " + biteMoveRotation);
        //Debug.Log("currentRotation " + currentRotation);
        float nextRotation = currentRotation + turnRotation;
        Debug.Log("abraham nextRotation " + nextRotation);
        //if (biteMoveRotation - 180 >= 0) { wrongDirection = biteMoveRotation - 180f; Debug.Log("abraham wrong direction " + wrongDirection); }
        //else if (biteMoveRotation + 180 <= 360f) wrongDirection = biteMoveRotation + 180f;
        // don't allow the snake head to rotate into its tail
        float wrongDirection1 = biteMoveRotation - 180f;
        float wrongDirection2 = biteMoveRotation + 180f;
        if (nextRotation != wrongDirection1 && nextRotation != wrongDirection2) snakeHead.transform.Rotate(0, turnRotation, 0);
    }

    Vector3 RotationToMovementVector(float rotation)
    {
        // rotacije niso zmeraj tako kot bi si želel
        // 90.000001 --> pri rotaciji pride do float precision errors, zato zaokoržim
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

    float MovementVectorToRotation(Vector3 movementVector)
    {
        movementVector = movementVector.normalized;
        if (movementVector == new Vector3(0f, 0f, 1f)) return 0f;
        else if (movementVector == new Vector3(1f, 0f, 0f)) return 90f;
        else if (movementVector == new Vector3(0f, 0f, -1f)) return 180f;
        else if (movementVector == new Vector3(-1f, 0f, 0f)) return 270f;
        return -1f;
    }

    void UpdateIndicator()
    {
        RectTransform arrowRect = arrow.GetComponent<RectTransform>();
        arrowRect.sizeDelta = new Vector2(arrowRect.sizeDelta.x, currentCalculatedBiteRange);
    }
    /*
    void PerformBiteLinecast()
    {
        Vector3 startPoint = Vector3.zero;
        Vector3 endPoint = startPoint + Vector3.forward * currentCalculatedBiteRange;
        Debug.Log("currentCalculatedBiteRange " + currentCalculatedBiteRange);
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);
        RaycastHit hit;
        if(Physics.Linecast(startPoint, endPoint, out hit))
        {
            Debug.Log($"Bite hit: {hit.collider.name} at distance {hit.distance}");
        }
    }
    */
    void PerformBiteLinecast()
    {
        Vector3 startPoint = snakeHead.transform.position;
        Vector3 endPoint = startPoint + snakeHead.transform.forward * currentCalculatedBiteRange;

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);

        RaycastHit hit;
        if (Physics.Linecast(startPoint, endPoint, out hit, layersToHit))
        {
            Debug.Log($"Bite hit: {hit.collider.name} at distance {hit.distance}");
        }
    }
}
