using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelSelector
{
    public List<Level> easyLevels;
    public List<Level> mediumLevels;
    public List<Level> hardLevels;
    public LevelSelector()
    {
        easyLevels = new List<Level>();
        mediumLevels = new List<Level>();
        hardLevels = new List<Level>();
        GetAllLevels();
    }
    public void GetAllLevels()
    {
        Level[] allLevels = Resources.LoadAll<Level>("");
        foreach (Level level in allLevels)
        {
            if (level != null)
            {
                if (level.difficulty == Difficulty.Easy) easyLevels.Add(level);
                else if (level.difficulty == Difficulty.Medium) mediumLevels.Add(level);
                else if (level.difficulty == Difficulty.Hard) hardLevels.Add(level);
            }
        }
        
    }
}
