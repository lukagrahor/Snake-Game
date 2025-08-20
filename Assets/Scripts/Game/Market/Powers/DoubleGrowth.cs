using UnityEngine;

public class DoubleGrowth : Power
{
    public override int Price { get => 1;}

    public void Start()
    {
        ItemMarket = transform.parent.GetComponent<Market>();
        defaultColor = meshRenderer.sharedMaterial.color;
        gameObject.name = "Double growth";
    }

    public override void Buy()
    {
        bool enoughMoney = ItemMarket.CheckForFunds(Price);
        if (enoughMoney)
        {
            Debug.Log("Double growth: bought");
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
        SnakeHead snakeHead = snake.SnakeHead;
        snakeHead.GrowthMultiplier = 2;
    }
}
