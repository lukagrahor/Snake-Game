using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    [SerializeField] AudioSource snakeEatSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        FoodActions.EatenByPlayer += PlaySnakeEatEffect;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlaySnakeEatEffect()
    {
        snakeEatSound.Play();
    }

    private void OnDestroy()
    {
        FoodActions.EatenByPlayer -= PlaySnakeEatEffect;
    }
}
