using UnityEngine;

public class DoubleGrowth : Power
{
    public override int Price { get => 8;}
    [SerializeField] MeshRenderer meshRenderer;

    public void Start()
    {
        ItemMarket = transform.parent.GetComponent<Market>();
    }

    public override void Buy()
    {
        Debug.Log("DoubleGrowth: bought");
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
