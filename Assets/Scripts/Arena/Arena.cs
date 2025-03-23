using Unity.VisualScripting;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [SerializeField] ArenaBlock arenaBlockA;
    [SerializeField] ArenaBlock arenaBlockB;
    [SerializeField] int size = 10;
    
    CameraCornerSpawner cameraCornerSpawner;
    ArenaBounds arenaBounds;
    ArenaBlock arenaBlock;

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

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float rowPosition = (i * blockSize) - 5f;
                float colPosition = (j * blockSize) - 5f;
                Vector3 location = new(colPosition, 0f, rowPosition);

                arenaBlock = ChooseArenaBlock(i, j);

                GameObject block = arenaBlock.Spawn(arenaBlock.gameObject, location, Quaternion.identity);
                block.transform.SetParent(transform);

                if (i == 0 && j == 0)
                {
                    bottom = location;
                    block.name = "bottom-corner";
                }

                if (i == 0 &&  j == (size - 1))
                {
                    right = location;
                    block.name = "right-corner";
                }

                if (i == (size - 1) && j == 0)
                {
                    left = location;
                    block.name = "left-corner";
                }

                if (i == (size - 1) && j == (size - 1))
                {
                    top = location;
                    block.name = "top-corner";
                }
            }
        }
        SetCornerBlockPositions(bottom, left, right, top);
    }

    ArenaBlock ChooseArenaBlock(int i, int j)
    {
        if (i % 2 == 0)
        {
            if (j % 2 == 0)
            {
                return arenaBlockA;
            }
            else
            {
                return arenaBlockB;
            }
        }
        else
        {
            if (j % 2 == 0)
            {
                return arenaBlockB;
            }
            else
            {
                return arenaBlockA;
            }
        }
    }

    /*
    ArenaBlock ChooseArenaBlock(int i)
    {
        int row = i / size;
        int rowOffset = row % 2 == 0 ? 1 : 0;
        int blockPosition = i + rowOffset;

        if (blockPosition % 2 == 0)
        {
            return arenaBlockA;
        }
        else
        {
            return arenaBlockB;
        }
    }*/

    void SetCornerBlockPositions(Vector3 bottom, Vector3 left, Vector3 right, Vector3 top) { cornerBlocks = new CornerBlocks(bottom, left, right, top); }

    public CornerBlocks GetCornerBlocks() { return cornerBlocks; }

    public int GetSize() { return size; }

    public float GetBlockSize() { return arenaBlockA.GetBlockSize(); }
}
