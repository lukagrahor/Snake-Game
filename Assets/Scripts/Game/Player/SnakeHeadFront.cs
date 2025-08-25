using System.Collections;
using UnityEngine;

public class SnakeHeadFront : MonoBehaviour
{
    [SerializeField] SnakeHead snakeHead;
    [SerializeField] SpitParticle spitPrefab;
    bool wait = false;

    private void OnTriggerEnter(Collider other)
    {
        var enteredObject = other.GetComponent<IFrontTriggerHandler>();
        enteredObject?.HandleFrontTrigger();
    }
    
    public void Spit()
    {
        if (spitPrefab == null) return;
        if (wait) return;
        SpitParticle newSpit = Instantiate(spitPrefab, transform);
        newSpit.transform.position = transform.position;
        newSpit.transform.SetParent(null);
        wait = true;
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(2f);
        wait = false;
    }
}
