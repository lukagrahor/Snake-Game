using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Power : MonoBehaviour, IPointerClickHandler
{
    public Market ItemMarket { get; set; }
    float rotationSpeed = 12f;
    public abstract int Price { get; }
    public abstract void Buy();
    public abstract void NotEnoughFunds();
    public abstract void SelectItem();
    //public abstract void Hover();

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Heeeeej");
        SelectItem();
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
