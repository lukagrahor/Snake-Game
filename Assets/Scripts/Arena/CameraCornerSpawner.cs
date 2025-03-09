using UnityEngine;
public class CameraCornerSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject bottom;
    [SerializeField] GameObject top;
    [SerializeField] GameObject left;
    [SerializeField] GameObject right;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup()
    {
        Arena arena = GetComponent<Arena>();
        Arena.CornerBlocks cornerBlocks = arena.GetCornerBlocks();

        int arenaSize = arena.GetSize();
        // 10 is the default arena size
        // each size bigger than 10 needs to add a few pixels to the corners in the target group
        // this subtraction ensures that the offset is 0 at arenaSize 10
        float targetGroupOffset = arenaSize - 10;
        if (targetGroupOffset != 0 && targetGroupOffset != 1)
        {
            targetGroupOffset = (Mathf.Pow(targetGroupOffset, 2) / 100) + 1/Mathf.Pow(targetGroupOffset, 4);
        }
        else if(targetGroupOffset == 1)
        {
            targetGroupOffset = 0.1f;
        }

        bottom.transform.position = cornerBlocks.bottom + new Vector3(-1.4f - targetGroupOffset, 1.4f, -1.4f - targetGroupOffset);
        top.transform.position = cornerBlocks.top + new Vector3(1.4f + targetGroupOffset, -0.2f, 1.4f + targetGroupOffset);
        left.transform.position = cornerBlocks.left + new Vector3(-1.5f - targetGroupOffset, 0f, 0f + targetGroupOffset);
        right.transform.position = cornerBlocks.right + new Vector3(1f + targetGroupOffset, 0f, -0.5f - targetGroupOffset);
    }
}
