using UnityEngine;

public class ArenaBounds : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float boundsSize;
    [SerializeField] ArenaWall wall;
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

    public void Spawn(Vector3 bottom, Vector3 left, Vector3 right, Vector3 top)
    {
        float blockSize = GetComponent<Arena>().GetBlockSize();

        float rightWallZ = (Mathf.Abs(bottom.z) + Mathf.Abs(right.z)) / 2f;
        rightWallZ += bottom.z;

        Vector3 rightWallPosition = new Vector3(bottom.x, blockSize, rightWallZ);

    }
}
