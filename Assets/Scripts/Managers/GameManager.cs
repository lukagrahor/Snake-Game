using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] Arena arena;
    LevelSelector levelSelector;
    void Awake()
    {
        CheckIfOnlyInstance();
        levelSelector = new LevelSelector();
        levelSelector.AddAllLevels();
    }

    void CheckIfOnlyInstance()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NewGame()
    {

    }

    public void GameOver()
    {

    }
}
