using UnityEngine;

public class Spit : Power
{
    [SerializeField] MeshRenderer meshRenderer;
    public override int Price { get => 6; }
    bool isSelected = false;

    public void Start()
    {
        ItemMarket = transform.parent.GetComponent<Market>();
    }

    public override void Buy()
    {
        Debug.Log("Split: bought");
        //Destroy(gameObject);
    }

    public override void SelectItem()
    {
        bool enoughMoney = ItemMarket.CheckForFunds(Price);
        meshRenderer.material.color = Color.yellow;
        meshRenderer.material.SetColor("_EmissionColor", Color.yellow);
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
