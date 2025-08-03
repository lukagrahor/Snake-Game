using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Scriptable Objects/Level")]
public class Level : ScriptableObject
{
    public int arenaSize;
    public Difficulty difficulty;
    public List<IntPair> wallObjectIndexes;
}