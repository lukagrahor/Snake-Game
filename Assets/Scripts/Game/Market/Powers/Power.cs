using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Power : MonoBehaviour, IPointerClickHandler
{
    float rotationSpeed = 12f;
    public abstract void Buy();
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Heeeeej");
        Buy();
    }

    public void RotateItem()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
