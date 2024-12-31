using Unity.VisualScripting;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [SerializeField] GameObject arenaBlock;
    [SerializeField] int size = 10;
    CameraCornerSpawner cameraCornerSpawner;
    public struct CornerBlocks
    {
        public Vector3 bottom, left, right, top;
        public CornerBlocks(Vector3 bottom, Vector3 left, Vector3 right, Vector3 top)
        {
            this.bottom = bottom;
            this.left = left;
            this.right = right;
            this.top = top;
        }
    }
    private CornerBlocks cornerBlocks;
    void Start()
    {
        SpawnArena();
        cameraCornerSpawner = GetComponent<CameraCornerSpawner>();
        cameraCornerSpawner.Setup();
    }

    void Update()
    {
        
    }

    void SpawnArena()
    {
        Vector3 bottom = Vector3.zero;
        Vector3 left = Vector3.zero;
        Vector3 right = Vector3.zero;
        Vector3 top = Vector3.zero;
        int colNumber = 0;
        for (int i = 0; i < size * size; i++)
        {
            colNumber = i % size;
            
            Vector3 location = new Vector3(colNumber - 5, 0f, (i / size) - 5);
            //Debug.Log($"i: {i} location: {location}");
            GameObject block = Instantiate(arenaBlock, location, Quaternion.identity);
            block.transform.SetParent(transform);

            if (i == 0)
            {
                bottom = location;
            }

            if (i == size-1)
            {
               left = location;
            }

            if (i == (size * size) - size)
            {
                right = location;
            }

            if (i == (size * size) -1)
            {
                top = location;
            }
        }
        SetCornerBlockPositions(bottom, left, right, top);
    }

    void SetCornerBlockPositions(Vector3 bottom, Vector3 left, Vector3 right, Vector3 top)
    {
        cornerBlocks = new CornerBlocks(bottom, left, right, top);
        Debug.Log($"left {cornerBlocks.left}");
    }

    public CornerBlocks GetCornerBlocks()
    {
        return cornerBlocks;
    }

    public int GetSize() { return size; }
}
