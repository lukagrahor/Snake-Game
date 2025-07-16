using UnityEngine;

public class PickupRotation : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 15f;
    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(0, Time.deltaTime * rotateSpeed, 0);
    }
}
