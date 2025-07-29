using UnityEngine;

public class DogFront : MonoBehaviour
{
    [SerializeField] Dog dog;
    private void OnTriggerEnter(Collider other)
    {
        IDogFrontTriggerHandler enemyFrontTrigger = other.GetComponent<IDogFrontTriggerHandler>();
        enemyFrontTrigger?.HandleDogTrigger(dog);
    }
}
