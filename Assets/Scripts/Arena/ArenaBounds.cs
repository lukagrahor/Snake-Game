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

    private void Start()
    {

    }

    public float GetBoundsSize()
    {
        return boundsSize;
    }
    public void SetBoundsSize(float boundsSize)
    {
        this.boundsSize = boundsSize;
    }

    public void Setup(Vector3 bottom, Vector3 left, Vector3 right, Vector3 top, float blockSize, float arenaSize)
    {
        bottomLeftWall = SpawnBottomLeftTopRightWalls(left, bottom, -1f, blockSize, arenaSize);
        topRightWall = SpawnBottomLeftTopRightWalls(top, right, 1f, blockSize, arenaSize);
        topLeftWall = SpawnTopLeftBottomRightWalls(left, top, 1f, blockSize, arenaSize);
        bottomRightWall = SpawnTopLeftBottomRightWalls(bottom, right, -1f, blockSize, arenaSize);

        bottomLeftWall.name = "bottom-left wall";
        topRightWall.name = "top-right wall";
        topLeftWall.name = "top-left wall";
        bottomRightWall.name = "bottom-right wall";
    }

    ArenaWall SpawnBottomLeftTopRightWalls(Vector3 leftEdge, Vector3 rightEdge, float xPositionDirection, float blockSize, float arenaSize)
    {
        float overlapOffset = 2f * blockSize;

        float zPosition = (Mathf.Abs(rightEdge.z) + Mathf.Abs(leftEdge.z)) / 2f;
        zPosition += rightEdge.z;

        Vector3 wallPosition = new Vector3(leftEdge.x + (blockSize * xPositionDirection), blockSize, zPosition);
        ArenaWall newWall = Instantiate(wall, wallPosition, Quaternion.identity);
        newWall.transform.localScale = new Vector3(blockSize, blockSize, arenaSize * blockSize + overlapOffset);

        return newWall;
    }
    
    ArenaWall SpawnTopLeftBottomRightWalls(Vector3 leftEdge, Vector3 rightEdge, float zPositionDirection, float blockSize, float arenaSize)
    {
        float topLeftWallX = (Mathf.Abs(rightEdge.x) + Mathf.Abs(leftEdge.x)) / 2f;
        topLeftWallX += leftEdge.x;

        Vector3 wallPosition = new Vector3(topLeftWallX, blockSize, rightEdge.z + (blockSize * zPositionDirection));
        ArenaWall newWall = Instantiate(wall, wallPosition, Quaternion.identity);
        newWall.transform.localScale = new Vector3(arenaSize * blockSize, blockSize, blockSize);

        return newWall;
    }
}
