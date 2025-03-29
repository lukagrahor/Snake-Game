using UnityEngine;

public class TestEnemyFront : MonoBehaviour
{
    [SerializeField] TestEnemy enemy;
    private void OnTriggerEnter(Collider other)
    {
        ITestEnemyFrontTrigger enemyFrontTrigger = other.GetComponent<ITestEnemyFrontTrigger>();
        enemyFrontTrigger?.HandleEnemyFrontTrigger(enemy);
    }
}
