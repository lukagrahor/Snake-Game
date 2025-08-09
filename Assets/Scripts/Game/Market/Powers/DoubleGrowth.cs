using UnityEngine;

public class DoubleGrowth : Power
{
    public override void Buy()
    {
        Debug.Log("Double growth bought");
        Destroy(gameObject);
    }

    /*
    public override void Hover()
    {
        Debug.Log("Split hovered");
    }
    */

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotateItem();
    }
}
