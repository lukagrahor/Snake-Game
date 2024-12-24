using System;
using UnityEngine;
using UnityEngine.Events;

public class SnakeHead : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] UnityEvent pickupItem;
    void Start()
    {
        Debug.Log("Head attached");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        pickupItem.Invoke();
    }

    void Move()
    {

    }
}
