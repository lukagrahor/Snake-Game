using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SnakeHead : MonoBehaviour, ISnakePart
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] UnityEvent pickupItem;
    //float moveRotationY = 0f;
    float moveSpeed = 0f;
    bool lastSnakePart = true;
    private bool hasSnapped = false;
    LinkedList<float> rotationBuffer;
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

    public void Setup(float moveSpeed, float moveRotation, Transform parentTransform, float arenaBlockSize)
    {
        transform.SetParent(parentTransform);
        // arena je na poziciji 0, kocka arene je velika 1, kar pomeni da gre za 0.5 gor od 0, kocka od kaèe pa je velika 0.5 --> 0.25
        transform.localPosition = new Vector3(0, arenaBlockSize, 0);

        SetMoveSpeed(moveSpeed);
        Debug.Log($"moveSpeed: {moveSpeed}");
        AddToRotationBuffer(moveRotation);
        SetRotation();
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"Collision: {other.GetComponent<Food>()}");
        if (other.GetComponent<Food>() != null)
        {
            pickupItem.Invoke();
        }

        if (other.GetComponent<GridObject>() != null)
        {
            hasSnapped = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log($"Collision: {other.GetComponent<Food>()}");
        if (other.GetComponent<GridObject>() != null)
        {
            //Debug.Log($"Kaèa glava pozicija {transform.position}");
            // ignore the y axis
            Vector3 gridBlockPosition = new Vector3(other.transform.position.x, 0f, other.transform.position.z);
            Vector3 snakeHeadPosition = new Vector3(transform.position.x, 0f, transform.position.z);

            Debug.Log($"Distance: {Vector3.Distance(snakeHeadPosition, gridBlockPosition)}");

            if (Vector3.Distance(snakeHeadPosition, gridBlockPosition) <= 0.05f && hasSnapped == false)
            {
                Debug.Log("Jabadabadu1");
                if (rotationBuffer.Count > 0)
                {
                    transform.position = new Vector3(gridBlockPosition.x, transform.position.y, gridBlockPosition.z);
                    hasSnapped = true;
                    SetRotation();
                }
            }

            //Debug.Log($"Mreža objekt pozicija {gridObjectTransform.position}");
            /*
            float headRotation = GetRotation();
            if (headRotation == 0)
            {
                if (transform.position.z >= gridObjectTransform.position.z)
                {
                    Debug.Log("Eureka1!!!");
                }
            }

            else if(headRotation == 180)
            {
                if (transform.position.z <= gridObjectTransform.position.z)
                {
                    Debug.Log("Eureka2!!!");
                }
            }

            else if (headRotation == 90)
            {
                if (transform.position.x >= gridObjectTransform.position.x)
                {
                    Debug.Log("Eureka3!!!");
                }
            }

            else if (headRotation == 270)
            {
                if (transform.position.x <= gridObjectTransform.position.x)
                {
                    Debug.Log("Eureka4!!!");
                }
            }*/

        }
    }

    public void Move()
    {
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.forward); // Vector3.forward --> local space, tranform.forward --> world space
    }

    public void SetRotation()
    {
        //Debug.Log($"snakeheadRotation1: {transform.rotation.eulerAngles}");
        if (rotationBuffer.Count <= 0)
        {
            return;
        }
        Debug.Log($"Prva rotacija v bufferju: {rotationBuffer.First.Value}");
        transform.Rotate(0, rotationBuffer.First.Value, 0);
        rotationBuffer.RemoveFirst();
        if (rotationBuffer.Count <= 0)
        {
            return;
        }
        Debug.Log($"Prva rotacija v bufferju po brisanju: {rotationBuffer.First.Value}");

        //Debug.Log($"snakeheadRotation2: {transform.rotation.eulerAngles}");
        //moveRotationY = rotation;
    }

    public void AddToRotationBuffer(float rotation)
    {
        Debug.Log($"Dodaj v buffer: {rotation}");
        rotationBuffer.AddLast(rotation);
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
}
