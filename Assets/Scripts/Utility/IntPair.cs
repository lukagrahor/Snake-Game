using UnityEngine;

[System.Serializable]
public struct IntPair
{
    public int Row;
    public int Col;

    public IntPair(int row, int col)
    {
        Row = row;
        Col = col;
    }
}
