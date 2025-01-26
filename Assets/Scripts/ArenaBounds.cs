using UnityEngine;

public class ArenaBounds : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float boundsSize;
    /*
    public GameObject Spawn(GameObject blockPrefab, Vector3 location, Quaternion rotation)
    {
        GameObject block = Instantiate(blockPrefab, location, rotation);
        block.transform.localScale = new Vector3(blockSize + 0.001f, blockSize + 0.001f, blockSize + 0.001f);
        return block;
    }*/

    public float GetBoundsSize()
    {
        return boundsSize;
    }
    public void SetBoundsSize(float boundsSize)
    {
        this.boundsSize = boundsSize;
    }
}
