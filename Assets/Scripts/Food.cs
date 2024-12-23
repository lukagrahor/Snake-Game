using UnityEngine;
using UnityEngine.Events;
public class Food : MonoBehaviour, IPickup
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    GameObject foodObject;
    [SerializeField] UnityEvent growSnake;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use() {
        if (foodObject != null)
        {
            Destroy(foodObject);
        }
        growSnake.Invoke();
    }
    public void Spawn() {
        foodObject = Instantiate(this.gameObject);
    }
}
