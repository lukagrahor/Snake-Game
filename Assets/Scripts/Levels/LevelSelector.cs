using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelSelector
{
    public List<Level> easyLevels;
    public List<Level> mediumLevels;
    public List<Level> hardLevels;
    LevelSelector()
    {

    }
    public void AddAllLevels()
    {
        string[] guids = AssetDatabase.FindAssets("t:Level"); // "t:Level" searches for assets of type Level

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Level level = AssetDatabase.LoadAssetAtPath<Level>(assetPath);
            if (level != null)
            {
                easyLevels.Add(level);
                Debug.Log("Ohja " + level.ToString());
            }
        }
    }
}
