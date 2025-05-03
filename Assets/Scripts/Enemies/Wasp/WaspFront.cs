using UnityEngine;

public class WaspFront : MonoBehaviour
{
    [SerializeField] Wasp wasp;
    private void OnTriggerEnter(Collider other)
    {
        IWaspFrontTriggerHandler enteredObject = other.GetComponent<IWaspFrontTriggerHandler>();
        enteredObject?.HandleTrigger(wasp);
    }
}
