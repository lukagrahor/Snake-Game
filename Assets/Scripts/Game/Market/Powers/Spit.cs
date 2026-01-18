using UnityEngine;

public class Spit : Power
{
    public override int Price { get => 6; }

    public void Start()
    {
        ItemMarket = transform.parent.GetComponent<Market>();
        defaultColor = meshRenderer.sharedMaterial.color;
        gameObject.name = "Spit";
    }

    public override void Buy()
    {
        bool enoughMoney = ItemMarket.CheckForFunds(Price);
        Debug.Log($"enoughMoney: {enoughMoney}");
        if (enoughMoney)
        {
            Debug.Log("Spit: bought");
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
        SnakeHeadStateMachine stateMachine = snake.SnakeHead.StateMachine;
        stateMachine.PowerState = stateMachine.SpittingState;
    }
}
