using UnityEngine;

public class BeeFront : MonoBehaviour
{
    [SerializeField] Bee enemy;
    private void OnTriggerEnter(Collider other)
    {
        IBeeFrontTriggerHandler enemyFrontTrigger = other.GetComponent<IBeeFrontTriggerHandler>();
        enemyFrontTrigger?.HandleEnemyFrontTrigger(enemy);
    }
}
