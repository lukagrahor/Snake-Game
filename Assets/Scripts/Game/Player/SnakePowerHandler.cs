using UnityEngine;

public class SnakePowerHandler : MonoBehaviour
{
    // This class oversees the snake's powers
    // active and passive abilities
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Snake snake;
    bool doubleGrowthActive = false;
    public bool DoubleGrowthActive { get => doubleGrowthActive; set => doubleGrowthActive = value; }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
