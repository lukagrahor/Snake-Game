using UnityEngine;

public class ArenaKillBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IArenaKillBoxTriggerHandler>()?.HandleKillBoxTrigger();
    }
}
