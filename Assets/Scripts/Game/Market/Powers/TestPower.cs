using UnityEngine;
using UnityEngine.EventSystems;

public class TestPower : Power
{
    [SerializeField] MeshRenderer meshRenderer;
    public override int Price { get => 1; }

    public void Start()
    {
        ItemMarket = transform.parent.GetComponent<Market>();
    }

    public override void Buy()
    {
        Debug.Log("Test item: bought");
        //Destroy(gameObject);
    }

    public override void SelectItem()
    {
        bool enoughMoney = ItemMarket.CheckForFunds(Price);
        if (enoughMoney) Buy();
        else NotEnoughFunds();
    }

    public override void NotEnoughFunds()
    {
        Debug.Log("Split: Nimaš dovolj denarja");
    }

    /*
    public override void Hover()
    {
        Debug.Log("Split hovered");
    }
    */

    // Update is called once per frame
    void Update()
    {
        RotateItem();
    }
}
