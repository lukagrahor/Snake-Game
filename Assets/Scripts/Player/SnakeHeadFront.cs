using UnityEngine;

public class SnakeHeadFront : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var enteredObject = other.GetComponent<IFrontTriggerHandler>();
        enteredObject?.HandleFrontTrigger();
    }
}
