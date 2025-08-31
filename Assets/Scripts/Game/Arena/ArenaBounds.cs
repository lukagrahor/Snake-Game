using UnityEngine;

public class ArenaBounds : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float boundsSize;
    [SerializeField] ArenaWall wall;
    [SerializeField] ArenaKillBox killBox;
    [SerializeField] Arena arena;
    ArenaWall topRightWall;
    ArenaWall bottomLeftWall;
    ArenaWall topLeftWall;
    ArenaWall bottomRightWall;

    ArenaKillBox topRightKillBox;
    ArenaKillBox bottomLeftKillBox;
    ArenaKillBox topLeftKillBox;
    ArenaKillBox bottomRightKillBox;

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

        bottomLeftKillBox = SpawnBottomLeftTopRightKillBoxes(left, bottom, -1f, blockSize, arenaSize, -0.12f);
        topRightKillBox = SpawnBottomLeftTopRightKillBoxes(top, right, 1f, blockSize, arenaSize, 0.12f);
        topLeftKillBox = SpawnTopLeftBottomRightKillBoxes(left, top, 1f, blockSize, arenaSize, 0.12f);
        bottomRightKillBox = SpawnTopLeftBottomRightKillBoxes(bottom, right, -1f, blockSize, arenaSize, -0.12f);

        bottomLeftKillBox.name = "bottom-left kill-box";
        topRightKillBox.name = "top-right kill-box";
        topLeftKillBox.name = "top-left kill-box";
        bottomRightKillBox.name = "bottom-right kill-box";
    }

    ArenaWall SpawnBottomLeftTopRightWalls(Vector3 leftEdge, Vector3 rightEdge, float xPositionDirection, float blockSize, float arenaSize)
    {
        float overlapOffset = 2f * blockSize;

        float halfDistance = (rightEdge.z - leftEdge.z) / 2f;
        float zPosition = leftEdge.z + halfDistance;

        Vector3 wallPosition = new (leftEdge.x + (blockSize * xPositionDirection), blockSize, zPosition);
        ArenaWall newWall = Instantiate(wall, wallPosition, Quaternion.identity);
        newWall.transform.localScale = new Vector3(blockSize, blockSize, arenaSize * blockSize + overlapOffset);
        newWall.transform.SetParent(arena.transform);

        return newWall;
    }
    
    ArenaWall SpawnTopLeftBottomRightWalls(Vector3 leftEdge, Vector3 rightEdge, float zPositionDirection, float blockSize, float arenaSize)
    {
        float halfDistance = (rightEdge.x - leftEdge.x) / 2f;
        float topLeftWallX = leftEdge.x + halfDistance;

        Vector3 wallPosition = new (topLeftWallX, blockSize, rightEdge.z + (blockSize * zPositionDirection));
        ArenaWall newWall = Instantiate(wall, wallPosition, Quaternion.identity);
        newWall.transform.localScale = new Vector3(arenaSize * blockSize, blockSize, blockSize);
        newWall.transform.SetParent(arena.transform);

        return newWall;
    }

    ArenaKillBox SpawnBottomLeftTopRightKillBoxes(Vector3 leftEdge, Vector3 rightEdge, float xPositionDirection, float blockSize, float arenaSize, float xOffset)
    {
        float overlapOffset = 2f * blockSize;

        float halfDistance = (rightEdge.z - leftEdge.z) / 2f;
        float zPosition = leftEdge.z + halfDistance;

        Vector3 wallPosition = new(leftEdge.x + (blockSize * xPositionDirection) + xOffset, blockSize, zPosition);
        ArenaKillBox newKillBox = Instantiate(killBox, wallPosition, Quaternion.identity);
        newKillBox.transform.localScale = new Vector3(blockSize, blockSize, arenaSize * blockSize + overlapOffset);
        newKillBox.transform.SetParent(arena.transform);

        return newKillBox;
    }

    ArenaKillBox SpawnTopLeftBottomRightKillBoxes(Vector3 leftEdge, Vector3 rightEdge, float zPositionDirection, float blockSize, float arenaSize, float zOffset)
    {
        float halfDistance = (rightEdge.x - leftEdge.x) / 2f;
        float topLeftWallX = leftEdge.x + halfDistance;

        Vector3 wallPosition = new(topLeftWallX, blockSize, rightEdge.z + (blockSize * zPositionDirection) + zOffset);
        ArenaKillBox newKillBox = Instantiate(killBox, wallPosition, Quaternion.identity);
        newKillBox.transform.localScale = new Vector3(arenaSize * blockSize, blockSize, blockSize);
        newKillBox.transform.SetParent(arena.transform);

        return newKillBox;
    }
}
