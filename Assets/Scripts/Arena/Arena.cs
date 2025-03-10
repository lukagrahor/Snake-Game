using Unity.VisualScripting;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [SerializeField] ArenaBlock arenaBlock;
    [SerializeField] int size = 10;
    
    CameraCornerSpawner cameraCornerSpawner;
    ArenaBounds arenaBounds;

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
    void Awake()
    {
        SpawnArena();

        arenaBounds = GetComponent<ArenaBounds>();
        arenaBounds.Setup(cornerBlocks.bottom, cornerBlocks.left, cornerBlocks.right, cornerBlocks.top, GetBlockSize(), size);

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

        float blockSize = GetBlockSize();
        for (int i = 0; i < size * size; i++)
        {
            float colPosition;
            float rowPosition;
            Vector3 location;

            rowPosition = ((i / size) * blockSize) -5;
            colPosition = (i * blockSize) % (size * blockSize);
            location = new Vector3(colPosition - 5, 0f, rowPosition);

            GameObject block = arenaBlock.Spawn(arenaBlock.gameObject, location, Quaternion.identity);
            block.transform.SetParent(transform);

            if (i == 0)
            {
                bottom = location;
            }

            if (i == size-1)
            {
               right = location;
            }

            if (i == (size * size) - size)
            {
                left = location;
            }

            if (i == (size * size) -1)
            {
                top = location;
            }
        }
        SetCornerBlockPositions(bottom, left, right, top);
    }

    void SetCornerBlockPositions(Vector3 bottom, Vector3 left, Vector3 right, Vector3 top) { cornerBlocks = new CornerBlocks(bottom, left, right, top); }

    public CornerBlocks GetCornerBlocks() { return cornerBlocks; }

    public int GetSize() { return size; }

    public float GetBlockSize() { return arenaBlock.GetBlockSize(); }
}
