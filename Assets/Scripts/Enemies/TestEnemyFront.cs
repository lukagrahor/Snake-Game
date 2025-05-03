using UnityEngine;

public class TestEnemyFront : MonoBehaviour
{
    [SerializeField] Bee enemy;
    private void OnTriggerEnter(Collider other)
    {
        IBeeFrontTriggerHandler enemyFrontTrigger = other.GetComponent<IBeeFrontTriggerHandler>();
        enemyFrontTrigger?.HandleEnemyFrontTrigger(enemy);
    }
}
