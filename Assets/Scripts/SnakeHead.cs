using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SnakeHead : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] UnityEvent pickupItem;
    //float moveRotationY = 0f;
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

    public void Setup(float moveSpeed, float moveRotation, Transform parentTransform)
    {
        transform.SetParent(parentTransform);
        // arena je na poziciji 0, kocka arene je velika 1, kar pomeni da gre za 0.5 gor od 0, kocka od kaèe pa je velika 0.5 --> 0.25
        transform.localPosition = new Vector3(0, 0.75f, 0);

        SetMoveSpeed(moveSpeed);
        SetRotation(moveRotation);
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
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.forward); // Vector3.forward --> local space, tranform.forward --> world space
    }

    public void SetRotation(float rotation)
    {
        //Debug.Log($"snakeheadRotation1: {transform.rotation.eulerAngles}");
        transform.Rotate(0, rotation, 0);
        //Debug.Log($"snakeheadRotation2: {transform.rotation.eulerAngles}");
        //moveRotationY = rotation;
    }

    public float GetRotation()
    {
        return transform.rotation.eulerAngles.y;
    }
}
