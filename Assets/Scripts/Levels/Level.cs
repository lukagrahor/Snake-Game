using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Scriptable Objects/Level")]
public class Level : ScriptableObject
{
    public float arenaSize;
    public Difficulty difficulty;
    public List<string> wallObjectIndexes;
    public enum Difficulty
    {
        Easy = 0,
        Medium = 1,
        Hard = 2
    }
}
