using UnityEngine;

public class ArenaBlock : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float blockSize = 0.4f;

    public GameObject Spawn(GameObject blockPrefab, Vector3 location, Quaternion rotation)
    {
        GameObject block = Instantiate(blockPrefab, location, rotation);
        block.transform.localScale = new Vector3(blockSize, blockSize, blockSize);
        return block;
    }

    public float GetBlockSize()
    {
        return blockSize;
    }
}
