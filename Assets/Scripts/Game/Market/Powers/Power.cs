using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Power : MonoBehaviour, IPointerClickHandler
{
    float rotationSpeed = 12f;
    public abstract void Buy();
    //public abstract void Hover();

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Heeeeej");
        Buy();
    }
    /*
    public void OnPointerEnter(PointerEventData eventData)
    {
        Hover();
    }
    */

    public void RotateItem()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
