using UnityEngine;

public class FlyFront : MonoBehaviour
{
    [SerializeField] Fly fly;
    private void OnTriggerEnter(Collider other)
    {
        IFlyFrontTriggerHandler enteredObject = other.GetComponent<IFlyFrontTriggerHandler>();
        enteredObject?.HandleFlyTrigger(fly);
    }
}
