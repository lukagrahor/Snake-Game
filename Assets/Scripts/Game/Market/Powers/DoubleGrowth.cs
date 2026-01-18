using UnityEngine;

public class DoubleGrowth : Power
{
    public override int Price { get => 4;}

    public void Start()
    {
        ItemMarket = transform.parent.GetComponent<Market>();
        defaultColor = meshRenderer.sharedMaterial.color;
        gameObject.name = "Double growth";
    }

    public override void Buy()
    {
        bool enoughMoney = ItemMarket.CheckForFunds(Price);
        Debug.Log($"enoughMoney: {enoughMoney}");
        if (enoughMoney)
        {
            Debug.Log($"Double growth item: bought {Price}");
            MarkNeededParts(Price);
            ItemMarket.EnableConfirmButton();
        }
        else NotEnoughFunds();

        //Destroy(gameObject);
    }

    public override void NotEnoughFunds()
    {
        Debug.Log("Split: Nimaš dovolj denarja");
        ItemMarket.DisableConfirmButton();
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
