using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected abstract void GetHit();
    public abstract void Setup(int col, int row, int gridSize);
}
