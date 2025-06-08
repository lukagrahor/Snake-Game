using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] Arena arena;
    [SerializeField] ArenaGrid grid;
    [SerializeField] SpawnerManager spawnerManager;
    LevelSelector levelSelector;
    Difficulty currentDifficulty = Difficulty.Easy;
    public Difficulty CurrentDifficulty { get => currentDifficulty; set => currentDifficulty = value; }
    int foodToCollect;
    int maxFoodCount;
    void Awake()
    {
        CheckIfOnlyInstance();
        levelSelector = new LevelSelector();
        List<Level> levels = new List<Level>();

        if (CurrentDifficulty == Difficulty.Easy)
        {
            levels = levelSelector.easyLevels;
            maxFoodCount = 5;
        }
        else if (CurrentDifficulty == Difficulty.Medium)
        {
            levels = levelSelector.mediumLevels;
            maxFoodCount = 8;
        }
        else if (CurrentDifficulty == Difficulty.Hard)
        {
            levels = levelSelector.hardLevels;
            maxFoodCount = 10;
        }

        foodToCollect = maxFoodCount;

        Level newLevel = ChooseALevel(levels);
        LinkedList<GridObject> wallBlocks = ArenaSetup(newLevel);
        spawnerManager.ManageFirstSpawns(wallBlocks);
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
        /*
        foreach (Level level in levels)
        {
            Debug.Log("andrej: " + level.name);
        }
        */
        int index = Random.Range(0, levels.Count);
        return levels[index];
    }
    LinkedList<GridObject> ArenaSetup(Level level)
    {
        Debug.Log("velikost arene " + level.arenaSize);
        arena.Size = level.arenaSize;
        arena.SpawnArena();
        grid.SetupGrid();
        LinkedList<GridObject> wallBlocks = grid.SpawnWalls(level.wallObjectIndexes);
        return wallBlocks;
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

    public void CollectFood()
    {
        foodToCollect--;
        if (foodToCollect != 0) return;
        // naslednji nivo
    }
}
