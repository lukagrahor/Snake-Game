using UnityEngine;

public class TestEnemyFront : MonoBehaviour
{
    [SerializeField] Bee enemy;
    private void OnTriggerEnter(Collider other)
    {
        IBeeFrontTrigger enemyFrontTrigger = other.GetComponent<IBeeFrontTrigger>();
        enemyFrontTrigger?.HandleEnemyFrontTrigger(enemy);
    }
}
