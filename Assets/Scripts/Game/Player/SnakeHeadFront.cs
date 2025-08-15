using UnityEngine;

public class SnakeHeadFront : MonoBehaviour
{
    [SerializeField] SnakeHead snakeHead;
    [SerializeField] SpitParticle spitPrefab;
    /*
    public void HandleEnemyFrontTrigger(StationaryEnemy enemy)
    {
        snakeHead.GetSnake().GetHit();
    }

    private void OnTriggerEnter(Collider other)
    {
        var enteredObject = other.GetComponent<IFrontTriggerHandler>();
        enteredObject?.HandleFrontTrigger();
    }
    */
    public void Spit()
    {
        if (spitPrefab == null) return;
        SpitParticle newSpit = Instantiate(spitPrefab, transform);
        newSpit.transform.position = transform.position;
        newSpit.transform.SetParent(null);
    }
}
