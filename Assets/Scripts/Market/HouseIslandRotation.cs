using UnityEngine;

public class HouseIslandRotation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float rotationSpeed = 10f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Time.deltaTime * rotationSpeed, 0);
    }
}
