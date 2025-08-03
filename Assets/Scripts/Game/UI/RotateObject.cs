using UnityEngine;

public class RotateObject : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(8f * Time.deltaTime, 30f * Time.deltaTime, 3f * Time.deltaTime);
    }
}
