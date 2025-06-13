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
        string[] guids = AssetDatabase.FindAssets("t:Level"); // "t:Level" searches for assets of type Level
        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Level level = AssetDatabase.LoadAssetAtPath<Level>(assetPath);
            if (level != null)
            {
                if (level.difficulty == Difficulty.Easy) easyLevels.Add(level);
                else if (level.difficulty == Difficulty.Medium) mediumLevels.Add(level);
                else if (level.difficulty == Difficulty.Hard) hardLevels.Add(level);
            }
        }
    }
}
