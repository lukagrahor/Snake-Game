using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SnakeHead : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] UnityEvent pickupItem;
    Vector3 moveDirection = new Vector3(0, 0, 0);
    float moveSpeed = 0f;
    void Start()
    {
        Debug.Log("Head attached");
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        pickupItem.Invoke();
    }

    void Move()
    {
        transform.Translate(moveDirection * Time.deltaTime);
    }

    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction * moveSpeed;
    }
}
