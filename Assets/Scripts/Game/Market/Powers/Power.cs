using UnityEngine;

public abstract class Power : MonoBehaviour
{
    float rotationSpeed = 12f;
    public abstract void Buy();

    public void RotateItem()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
