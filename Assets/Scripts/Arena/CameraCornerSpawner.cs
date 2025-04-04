using UnityEngine;
public class CameraCornerSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] CornerBlock bottom;
    [SerializeField] CornerBlock top;
    [SerializeField] CornerBlock left;
    [SerializeField] CornerBlock right;

    public void Setup()
    {
        Arena arena = GetComponent<Arena>();
        SetCornerPositions(arena);
        SetCornerPositions(arena);
    }

    float SetTargetGroupOffset(Arena arena)
    {
        int arenaSize = arena.GetSize();
        /*
        float targetGroupOffset = arenaSize - 10;
        if (targetGroupOffset != 0 && targetGroupOffset != 1)
        {
            targetGroupOffset = (Mathf.Pow(targetGroupOffset, 2) / 100) + 1 / Mathf.Pow(targetGroupOffset, 4);
        }
        else if (targetGroupOffset == 1)
        {
            targetGroupOffset = 0.1f;
        }*/

        return 0f;
    }

    // 10 is the default arena size
    // each size bigger than 10 needs to add a few pixels to the corners in the target group
    // this subtraction ensures that the offset is 0 at arenaSize 10
    void SetCornerPositions(Arena arena)
    {
        float targetGroupOffset = SetTargetGroupOffset(arena);
        Arena.CornerBlocks cornerBlocks = arena.GetCornerBlocks();

        bottom.transform.position = cornerBlocks.bottom + new Vector3(-1.4f - targetGroupOffset, 1.4f, -1.4f - targetGroupOffset);
        top.transform.position = cornerBlocks.top + new Vector3(1.4f + targetGroupOffset, -0.2f, -0.1f + targetGroupOffset);
        left.transform.position = cornerBlocks.left + new Vector3(-1.5f - targetGroupOffset, 0f, 0f + targetGroupOffset);
        right.transform.position = cornerBlocks.right + new Vector3(1f + targetGroupOffset, 0f, -0.5f - targetGroupOffset);
    }
}
