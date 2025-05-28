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
        Debug.Log("Ohja ---------------------------");
        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Level level = AssetDatabase.LoadAssetAtPath<Level>(assetPath);
            if (level != null)
            {
                easyLevels.Add(level);
                Debug.Log("Ohja " + level.ToString());
                Debug.Log("Ohja " + level.arenaSize);
                Debug.Log("Ohja " + level.difficulty);
            }
        }
    }
}
