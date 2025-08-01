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
    float chargeTimeLimit = 3f;
    float currentCalculatedBiteRange;
    float moveSpeed = 0.5f;
    LineRenderer lineRenderer;

    //Quaternion biteMoveRotation;
    Vector3 biteMoveDirecton;
    public SnakeBitingState(SnakeHead snakeHead, LayerMask layersToHit, SnakeHeadStateMachine stateMachine)
    {
        this.snakeHead = snakeHead;
        this.layersToHit = layersToHit;
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        snakeHead.Snake.MoveSpeed = moveSpeed;
        currentChargeTime = 0f;
        SetBiteMovementDirection();
        arrow = snakeHead.Arrow;
        arrow.SetActive(true);
        arrow.GetComponent<RectTransform>().sizeDelta = new Vector2(arrowWidth, minArrowLength);
        lineRenderer = snakeHead.LineRenderer;
    }

    public void Exit()
    {
        PerfromBiteBoxCast();
        snakeHead.transform.forward = biteMoveDirecton;
        arrow.SetActive(false);
    }

    public void Update()
    {
        currentChargeTime += Time.deltaTime;
        if (currentChargeTime >= chargeTimeLimit) stateMachine.TransitionTo(stateMachine.NormalState);
        float limitedChargeTime = Mathf.Min(currentChargeTime, maxChargeTime);

        currentCalculatedBiteRange = Mathf.Lerp(minArrowLength, maxArrowLength, limitedChargeTime / maxChargeTime);
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
        // ne sme se takoj prekinit, ker �e ne gre ka�a off the grid --> mogo�e bulj�e vrnt igralca nazaj na prej�njo rotacijo
        if (biteMoveDirecton == null) return;
        snakeHead.transform.Translate(moveSpeed * Time.deltaTime * biteMoveDirecton, Space.World);
    }

    public void SetRotation(float turnRotation)
    {
        float biteMoveRotation = MovementVectorToRotation(biteMoveDirecton);
        float currentRotation = snakeHead.GetRotation();
        float nextRotation = currentRotation + turnRotation;
        //if (biteMoveRotation - 180 >= 0) { wrongDirection = biteMoveRotation - 180f; Debug.Log("abraham wrong direction " + wrongDirection); }
        //else if (biteMoveRotation + 180 <= 360f) wrongDirection = biteMoveRotation + 180f;
        // don't allow the snake head to rotate into its tail
        float wrongDirection1 = biteMoveRotation - 180f;
        float wrongDirection2 = biteMoveRotation + 180f;
        if (nextRotation != wrongDirection1 && nextRotation != wrongDirection2) snakeHead.transform.Rotate(0, turnRotation, 0);
    }

    Vector3 RotationToMovementVector(float rotation)
    {
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

    void PerfromBiteBoxCast()
    {
        Vector3 halfExtents = new Vector3 (0.07f, 0.07f, 0.07f);
        Vector3 startPoint = snakeHead.transform.position;
        Vector3 endPoint = startPoint + snakeHead.transform.forward * currentCalculatedBiteRange;
        Vector3 direction = (endPoint - startPoint).normalized;
        float maxDistance = Vector3.Distance(startPoint, endPoint);
        Quaternion boxOrientation = snakeHead.transform.rotation;
        RaycastHit hit;
        if (Physics.BoxCast(startPoint, halfExtents, direction, out hit, boxOrientation, maxDistance, layersToHit))
        {
            IBiteTriggerHandler hitObject = hit.collider.gameObject.GetComponent<IBiteTriggerHandler>();
            hitObject?.HandleBiteTrigger(snakeHead);
        }
    }
}
