using UnityEngine;

public class EnemyFront : MonoBehaviour
{
    [SerializeField] StationaryEnemy enemy;
    private void OnTriggerEnter(Collider other)
    {
        IEnemyFrontTrigger enemyFrontTrigger = other.GetComponent<IEnemyFrontTrigger>();
        enemyFrontTrigger?.HandleEnemyFrontTrigger(enemy);
    }
}
