using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] Arena arena;
    [SerializeField] ArenaGrid grid;
    [SerializeField] SpawnerManager spawnerManager;
    [SerializeField] GameUIManager UIManager;
    [SerializeField] PrimaryCamera cam;
    [SerializeField] Snake snake;
    LevelSelector levelSelector;
    Difficulty currentDifficulty = Difficulty.Easy;
    int levelNumber = 1;
    int lastLevelNumber = 7;
    public Difficulty CurrentDifficulty { get => currentDifficulty; set => currentDifficulty = value; }
    void Awake()
    {
        CheckIfOnlyInstance();
        levelSelector = new LevelSelector();
        List<Level> levels = SelectLevel();
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
        int index = Random.Range(0, levels.Count);
        return levels[index];
    }
    LinkedList<GridObject> ArenaSetup(Level level)
    {
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

    public void MoveCamera()
    {
        cam.MoveCameraAway();
        snake.DespawnForNewLevel();
        snake.gameObject.SetActive(false);
    }

    public void StartNewLevel()
    {
        DespawnCurrentLevel();
        PrepareNewLevel();
    }

    void DespawnCurrentLevel()
    {
        // despawn all the enemies and objects
        spawnerManager.DespawnAllObjects();
        grid.DespawnInnerWalls();
        grid.DespawnGridObjects();
        // brez tega se ob pojavitvi nove arene, kamera takoj premakne na areno
        DisableCornerBlocks();
        // despawn the arena
        arena.DespawnArena();
    }

    void PrepareNewLevel()
    {
        // chose a new level and make a new arena
        levelNumber++;
        if (levelNumber == 3) CurrentDifficulty = Difficulty.Medium;
        else if (levelNumber == 6) CurrentDifficulty = Difficulty.Hard;
        spawnerManager.EnableSpawners();

        // spawn new enemies and objects
        List<Level> levels = SelectLevel();
        Level newLevel = ChooseALevel(levels);
        LinkedList<GridObject> wallBlocks = ArenaSetup(newLevel);
        spawnerManager.ManageNewLevelSpawns(wallBlocks);

        // spawn the player
        spawnerManager.SpawnPlayer();
    }

    void DisableCornerBlocks()
    {
        CornerBlock[] blocks = FindObjectsByType<CornerBlock>(FindObjectsSortMode.None);
        foreach (CornerBlock block in blocks)
        {
            block.gameObject.SetActive(false);
        }
    }

    List<Level> SelectLevel()
    {
        List<Level> levels = new List<Level>();

        if (CurrentDifficulty == Difficulty.Easy)
        {
            levels = levelSelector.easyLevels;
            UIManager.SetMaxFood(1);
        }
        else if (CurrentDifficulty == Difficulty.Medium)
        {
            levels = levelSelector.mediumLevels;
            UIManager.SetMaxFood(8);
        }
        else if (CurrentDifficulty == Difficulty.Hard)
        {
            levels = levelSelector.hardLevels;
            UIManager.SetMaxFood(10);
        }
        return levels;
    }

    void CheckForGameOver()
    {

    }

    public void GameOver()
    {

    }
}
