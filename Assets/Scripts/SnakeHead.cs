using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SnakeHead : MonoBehaviour, ISnakePart
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //public event Action onRotate;
    //float moveRotationY = 0f;
    float moveSpeed = 0f;
    bool lastSnakePart = true;
    private bool hasSnapped = false;
    LinkedList<float> rotationBuffer;
    // for debugging
    int blocksPassed = 0;
    float time = 0f;
    void Awake()
    {
        rotationBuffer = new LinkedList<float>();
        //Debug.Log("Head attached");
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void Setup(float moveSpeed, float moveRotation, Transform parentTransform, float arenaBlockSize, Snake snake)
    {
        //onRotate += snake.SetTorsoRotation;

        transform.SetParent(parentTransform);
        // arena je na poziciji 0, kocka arene je velika 1, kar pomeni da gre za 0.5 gor od 0, kocka od kaèe pa je velika 0.5 --> 0.25
        transform.localPosition = new Vector3(0f, arenaBlockSize - 0.05f, -2f);

        SetMoveSpeed(moveSpeed);
        //Debug.Log($"moveSpeed: {moveSpeed}");
        AddToRotationBuffer(moveRotation);
        SetRotation();
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GridObject>() != null)
        {
            //Debug.Log($"other {other.GetComponent<GridObject>().getId()}");
            time = Time.realtimeSinceStartup;
            hasSnapped = false;
            blocksPassed += 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<GridObject>() != null)
        {
            //Debug.Log($"Izhod iz grid kocke: {other.GetComponent<GridObject>().getId()}");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log($"Collision: {other.GetComponent<Food>()}");
        if (other.GetComponent<GridObject>() != null)
        {
            // ignore the y axis
            Vector3 gridBlockPosition = new Vector3(other.transform.position.x, 0f, other.transform.position.z);
            Vector3 snakeHeadPosition = new Vector3(transform.position.x, 0f, transform.position.z);

            //Debug.Log($"Distance: {Vector3.Distance(snakeHeadPosition, gridBlockPosition)}");
            // too small distance can cause the snake to not turn when needed
            if (Vector3.Distance(snakeHeadPosition, gridBlockPosition) <= 0.03f && hasSnapped == false)
            {
                //Debug.Log($"rotationBuffer count: {rotationBuffer.Count}");
                //Debug.Log("Jabadabadu1");
                if (rotationBuffer.Count > 0)
                {
                    // snap to the place of the grid block
                    transform.position = new Vector3(gridBlockPosition.x, transform.position.y, gridBlockPosition.z);
                    hasSnapped = true;

                    SetRotation();
                }
            }
        }
    }

    public void Move()
    {
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.forward); // Vector3.forward --> local space, transform.forward --> world space
    }

    public void SetRotation()
    {
        //Debug.Log($"snakeheadRotation1: {transform.rotation.eulerAngles}");
        if (rotationBuffer.Count <= 0)
        {
            return;
        }

        //Debug.Log("Rotiraj!");
        //Debug.Log($"Prva rotacija v bufferju: {rotationBuffer.First.Value}");
        transform.Rotate(0, rotationBuffer.First.Value, 0);
        time = Time.realtimeSinceStartup - time;
        //Debug.Log($"Head speed: {moveSpeed}");
        //Debug.Log($"Èas potreben, da head doseže lokacijo rotacije: {time}");
        rotationBuffer.RemoveFirst();
        transform.parent.GetComponent<Snake>().SetYRotation(GetRotation());
        //transform.parent.GetComponent<Snake>().setNextTorsoRotation(GetRotation());
        transform.parent.GetComponent<Snake>().SetTorsoRotation();

        /*if (onRotate != null)
        {
            onRotate();
        }*/

        //moveSpeed -= 0.03f;
        //Debug.Log($"blocksPassed: {blocksPassed}");

        //Debug.Log($"Prva rotacija v bufferju po brisanju: {rotationBuffer.First.Value}");

        //Debug.Log($"snakeheadRotation2: {transform.rotation.eulerAngles}");
        //moveRotationY = rotation;
    }

    public void AddToRotationBuffer(float rotation)
    {
        //Debug.Log($"Dodaj v buffer: {rotation}");
        // to prevent spam
        if (rotationBuffer.Count == 2)
        {
            return;
        }
        rotationBuffer.AddLast(rotation);
        //moveSpeed += 0.03f;
        blocksPassed = 0;
    }

    public float GetRotation()
    {
        return transform.rotation.eulerAngles.y;
    }
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

    public LinkedList<float> GetRotationBuffer()
    {
        return rotationBuffer;
    }
    public LinkedList<Vector3> GetPositionBuffer()
    {
        return new LinkedList<Vector3>();
    }
}
