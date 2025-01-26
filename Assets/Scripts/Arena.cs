using Unity.VisualScripting;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [SerializeField] ArenaBlock arenaBlock;
    [SerializeField] int size = 10;
    [SerializeField] ArenaBounds bounds;
    //[SerializeField] float blockSize = 0.5f;
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
        float colNumber = 0f;
        float blockSize = GetBlockSize();
        for (int i = 0; i < size * size; i++)
        {
            Vector3 location = Vector3.zero;
            if (blockSize < 1f)
            {
                colNumber = (i * blockSize) % (size * blockSize);
                location = new Vector3(colNumber - 5, 0f, ((i / size) * blockSize) - 5);
            }
            else
            {
                colNumber = i % size;
                location = new Vector3(colNumber - 5, 0f, (i / size) - 5);
            }

            //Debug.Log($"i: {i} location: {location}");
            GameObject block = arenaBlock.Spawn(arenaBlock.gameObject, location, Quaternion.identity);
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
        //Debug.Log($"left {cornerBlocks.left}");
    }

    public CornerBlocks GetCornerBlocks()
    {
        return cornerBlocks;
    }

    public int GetSize() { return size; }

    public float GetBlockSize()
    {
        return arenaBlock.GetBlockSize();
    }
}
