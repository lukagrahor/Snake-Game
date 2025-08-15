using UnityEngine;

public class Spit : Power
{
    public override int Price { get => 6; }

    public void Start()
    {
        ItemMarket = transform.parent.GetComponent<Market>();
        defaultColor = meshRenderer.material.color;
        gameObject.name = "Split";
    }

    public override void Buy()
    {
        bool enoughMoney = ItemMarket.CheckForFunds(Price);
        if (enoughMoney)
        {
            Debug.Log("Spit: bought");
            MarkNeededParts(Price);
        }
        else NotEnoughFunds();

        //Destroy(gameObject);
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

    public override void EnablePower(Snake snake)
    {
        Debug.Log("Enabled");
    }
}
