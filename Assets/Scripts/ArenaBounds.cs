using UnityEngine;

public class ArenaBounds : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float boundsSize;
    [SerializeField] ArenaWall wall;
    ArenaWall topRightWall;
    ArenaWall bottomLeftWall;
    ArenaWall topLeftWall;
    ArenaWall bottomRightWall;
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

    public void Setup(Vector3 bottom, Vector3 left, Vector3 right, Vector3 top)
    {
        float blockSize = GetComponent<Arena>().GetBlockSize();
        float arenaSize = GetComponent<Arena>().GetSize();
        float overlapOffset = 2f * blockSize;
        bottomLeftWall = SpawnBottomLeftTopRightWalls(left, bottom, -1f);
        topRightWall = SpawnBottomLeftTopRightWalls(top, right, 1f);
        topLeftWall = SpawnTopLeftBottomRightWalls(left, top, -1f);
        bottomRightWall = SpawnTopLeftBottomRightWalls(bottom, right, 1f);
        /*
        float bottomLeftWallZ = (Mathf.Abs(bottom.z) + Mathf.Abs(left.z)) / 2f;
        bottomLeftWallZ += bottom.z;

        Vector3 bottomLeftWallPosition = new Vector3(bottom.x - blockSize, blockSize, bottomLeftWallZ);
        bottomLeftWall = Instantiate(wall, bottomLeftWallPosition, Quaternion.identity);
        bottomLeftWall.transform.localScale = new Vector3(blockSize, blockSize, arenaSize * blockSize + overlapOffset);
        bottomLeftWall.name = "bottom-left wall";

        float topRightWallZ = (Mathf.Abs(top.z) + Mathf.Abs(right.z)) / 2f;
        topRightWallZ += right.z;

        Vector3 topRightWallPosition = new Vector3(top.x + blockSize, blockSize, topRightWallZ);
        topRightWall = Instantiate(wall, topRightWallPosition, Quaternion.identity);
        topRightWall.transform.localScale = new Vector3(blockSize, blockSize, arenaSize * blockSize + overlapOffset);
        topRightWall.name = "top-right wall";
        */
        float topLeftWallX = (Mathf.Abs(top.x) + Mathf.Abs(left.x)) / 2f;
        topLeftWallX += left.x;

        Vector3 topLeftWallPosition = new Vector3(topLeftWallX, blockSize, top.z + blockSize);
        topLeftWall = Instantiate(wall, topLeftWallPosition, Quaternion.identity);
        topLeftWall.transform.localScale = new Vector3(arenaSize * blockSize, blockSize, blockSize);
        topLeftWall.name = "top-left wall";

        float bottomRightWallX = (Mathf.Abs(bottom.x) + Mathf.Abs(right.x)) / 2f;
        bottomRightWallX += bottom.x;

        Vector3 bottomRightWallPosition = new Vector3(bottomRightWallX, blockSize, bottom.z - blockSize);
        bottomRightWall = Instantiate(wall, bottomRightWallPosition, Quaternion.identity);
        bottomRightWall.transform.localScale = new Vector3(arenaSize * blockSize, blockSize, blockSize);
        bottomRightWall.name = "bottom-right wall";
    }
    
    ArenaWall SpawnBottomLeftTopRightWalls(Vector3 leftEdge, Vector3 rightEdge, float xPositionDirection)
    {
        float blockSize = GetComponent<Arena>().GetBlockSize();
        float arenaSize = GetComponent<Arena>().GetSize();
        float overlapOffset = 2f * blockSize;

        float zPosition = (Mathf.Abs(rightEdge.z) + Mathf.Abs(leftEdge.z)) / 2f;
        zPosition += rightEdge.z;

        Vector3 wallPosition = new Vector3(leftEdge.x + (blockSize * xPositionDirection), blockSize, zPosition);
        ArenaWall newWall = Instantiate(wall, wallPosition, Quaternion.identity);
        newWall.transform.localScale = new Vector3(blockSize, blockSize, arenaSize * blockSize + overlapOffset);

        return newWall;
    }
    
    ArenaWall SpawnTopLeftBottomRightWalls(Vector3 leftEdge, Vector3 rightEdge, float xPositionDirection)
    {
        float blockSize = GetComponent<Arena>().GetBlockSize();
        float arenaSize = GetComponent<Arena>().GetSize();
        float overlapOffset = 2f * blockSize;

        float topLeftWallX = (Mathf.Abs(rightEdge.x) + Mathf.Abs(leftEdge.x)) / 2f;
        topLeftWallX += leftEdge.x;

        Vector3 wallPosition = new Vector3(topLeftWallX, blockSize, rightEdge.z + blockSize);
        ArenaWall newWall = Instantiate(wall, wallPosition, Quaternion.identity);
        newWall.transform.localScale = new Vector3(arenaSize * blockSize, blockSize, blockSize);

        return newWall;
    }
}
