using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] Arena arena;
    LevelSelector levelSelector;
    Difficulty currentDifficulty = Difficulty.Easy;
    public Difficulty CurrentDifficulty { get => currentDifficulty; set => currentDifficulty = value; }
    public enum Difficulty
    {
        Easy = 0,
        Medium = 1,
        Hard = 2
    }
    void Awake()
    {
        CheckIfOnlyInstance();
        levelSelector = new LevelSelector();
        List<Level> levels = new List<Level>();

        if (CurrentDifficulty == Difficulty.Easy) levels = levelSelector.easyLevels;
        else if (CurrentDifficulty == Difficulty.Medium) levels = levelSelector.mediumLevels;
        else if (CurrentDifficulty == Difficulty.Hard) levels = levelSelector.hardLevels;

        Level newLevel = ChooseALevel(levels);
        ArenaSetup(newLevel);
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
    Level ChooseALevel(List<Level> levels)
    {
        int index = Random.Range(0, levels.Count);
        return levels[index];
    }
    void ArenaSetup(Level level)
    {
        Debug.Log("velikost arene " + level.arenaSize);
        arena.Size = level.arenaSize;
        arena.SpawnArena();
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
