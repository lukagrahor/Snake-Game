using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Power : MonoBehaviour, IPointerClickHandler
{
    public MeshRenderer meshRenderer;
    public Color defaultColor;
    public Market ItemMarket { get; set; }
    float rotationSpeed = 12f;
    public bool isSelected = false;
    public abstract int Price { get; }
    public abstract void Buy();
    public abstract void NotEnoughFunds();
    public bool bought = false;
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

    public void SelectItem()
    {
        if (!isSelected)
        {
            // unselect all other items
            ItemMarket.UnselectPreviouslySelected(Price, this);

            meshRenderer.material.color = Color.yellow;
            meshRenderer.material.SetColor("_EmissionColor", Color.yellow);
            Buy();
            isSelected = true;
            return;
        }

        UnselectItem();
    }

    public void UnselectItem()
    {
        meshRenderer.material.color = defaultColor;
        meshRenderer.material.SetColor("_EmissionColor", Color.black);
        isSelected = false;
        ItemMarket.RemoveMarks(Price);
    }

    public void MarkNeededParts(int partCount)
    {
        ItemMarket.MarkParts(partCount);
    }
}
