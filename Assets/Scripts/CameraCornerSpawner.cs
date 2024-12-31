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
        Debug.Log($"cornerBlocks: {cornerBlocks.left}");
        
        bottom.transform.position = cornerBlocks.bottom + new Vector3(-0.5f, 0.2f, -0.5f);
        top.transform.position = cornerBlocks.top + new Vector3(0.5f, -0.2f, 0.5f);
        left.transform.position = cornerBlocks.left + new Vector3(1f, 0f, -1.5f);
        right.transform.position = cornerBlocks.right + new Vector3(-1.5f, 0f, 1f);
    }
}
