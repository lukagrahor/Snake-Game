using System;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UIElements;
using GlobalEnums;
using static UnityEngine.Splines.SplineInstantiate;
using System.Linq;

public class Snake : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] SnakeHead snakeHeadPrefab;
    [SerializeField] SnakeTorso snakeTorsoPrefab;
    [SerializeField] Canvas abilityChargeCanvas;
    [SerializeField] PlayerSpawner playerSpawner;
    //[SerializeField] SnakeCorner snakeCornerPrefab;
    List<SnakeTorso> snakeTorsoParts;
    //SnakeCorner snakeCorner;
     
    [SerializeField] ArenaBlock arenaBlock;
    [SerializeField] float snakeScale = 1.1f;
    float defaultSpeed;
    [SerializeField][Range(0, 7)] float moveSpeed = 2f;
    [SerializeField][Range(2, 6)] int startingSize = 2;
    Directions startingDirection = Directions.Up;
    Vector3 spawnPosition;

    [SerializeField] float waitTime = 3f;
    [SerializeField] CountDownTimer timer;
    ISnakeInput snakeInputManager;

    int minTorsoParts = 2;

    public SnakeHead SnakeHead { get; set; }
    public SnakePath Path;
    public float DefaultSpeed { get => defaultSpeed; set => defaultSpeed = value; }
    public float MoveSpeed
    {
        get => moveSpeed;
        set
        {
            moveSpeed = value;
            SnakeHead.MoveSpeed = value;
            SetTorsoSpeed();
        }
    }

    public int SnakeSize { get => snakeTorsoParts.Count; }
    public int NewLevelSize { get; set; }
    public int StartingSize { get => startingSize; }
    public Directions StartingDirection { get => startingDirection; set => startingDirection = value; }

    int blocksToSpawn = 0;

    public void FirstSpawn(Vector3 spawnPosition)
    {
        defaultSpeed = moveSpeed;
        snakeInputManager = CreateInputManager();
        snakeTorsoParts = new List<SnakeTorso>();
        // arena je na poziciji 0, kocka arene je velika 1, kar pomeni da gre za 0.5 gor od 0, kocka od kaèe pa je velika 0.5 --> 0.25
        //spawnPosition = new Vector3(-4.2f, arenaBlock.GetBlockSize()/2f + snakeScale/2f, -0.5999999f);
        spawnPosition.y = (arenaBlock.GetBlockSize() / 2f + snakeScale / 2f) - 0.02f;
        SnakeHead = Instantiate(snakeHeadPrefab.gameObject, spawnPosition, Quaternion.identity).GetComponent<SnakeHead>();
        Vector3 snakeScaleVector = new(snakeScale, snakeScale, snakeScale);

        SnakeHead.AbilityChargeCanvas = abilityChargeCanvas;
        SnakeHead.Setup(moveSpeed, this, snakeScaleVector);

        timer.TimeRanOut += Respawn;

        SpawnStartingTorsoBlocks();
        SnakeHead.SetStateMachine(); // need to set here, because if set earleir the torso parts won't be transparent
    }

    public void NewLevelSpawn(Vector3 spawnPosition)
    {
        Debug.Log("Spawnej kaèo");
        SnakeHead.transform.position = spawnPosition;
        gameObject.SetActive(true);
        SnakeHead.gameObject.SetActive(true);
        SpawnStartingTorsoBlocks(NewLevelSize);
        SnakeHead.SetStateMachine();
        /*
        foreach(SnakeTorso torso in snakeTorsoParts)
        {
            torso.gameObject.SetActive(true);
        }
        */
    }

    void SpawnStartingTorsoBlocks(int size = 0)
    {
        int numOfBlocks = size;
        if (size == 0) numOfBlocks = startingSize;
        if (blocksToSpawn > 0) numOfBlocks = blocksToSpawn;
        for (int i = 0; i < numOfBlocks; i++)
        {
            Grow();
        }
    }

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = -1;
    }

    public void Respawn()
    {
        snakeTorsoParts = new List<SnakeTorso>();
        //SnakeHead.transform.position = spawnPosition;
        playerSpawner.Spawn(); // sets the new position
        SnakeHead.gameObject.SetActive(true);
        snakeInputManager.OnSnakeRespawn();
        SpawnStartingTorsoBlocks();
        SnakeHead.SetStateMachine();
    }

    public float GetSnakeYRotation()
    {
        return SnakeHead.GetRotation();
    }

    public void SetNextYRotation(float turnRotation)
    {
        ISnakeState snakeState = (ISnakeState)SnakeHead.StateMachine.CurrentState;
        snakeState.SetRotation(turnRotation);
        //SnakeHead.AddToRotationBuffer(turnRotation);
    }

    /**
     * <summary>Returns what will be the snake head's rotation after it rotates by
     * the next rotation that is saved in the buffer</summary>
     * **/
    public float GetNextHeadRotation()
    {
        if (SnakeHead == null)
        {
            return -2f;
        }
        float currentRotation = SnakeHead.GetRotation();
        float nextRotation = currentRotation + SnakeHead.GetNextRotation();
        nextRotation = GetAbsoluteRotation(nextRotation);

        return nextRotation;
    }

    public void SetTorsoRotation(float nextTorsoRotation)
    {
        if (snakeTorsoParts.Count == 0)
        {
            return;
        }
        foreach (SnakeTorso torso in snakeTorsoParts)
        {
            torso.AddToRotationBuffer(nextTorsoRotation);
            torso.AddToPositionBuffer(SnakeHead.transform.position);
        }
    }

    private void SetTorsoSpeed()
    {
        foreach (SnakeTorso torso in snakeTorsoParts)
        {
            torso.MoveSpeed = MoveSpeed;
        }
    }

    public void Grow()
    {
        SnakeTorso newSnakeTorso = Instantiate(snakeTorsoPrefab.gameObject).GetComponent<SnakeTorso>();
        ISnakePart previousPart;
        float distanceFromParent = 0.9f;
        if (snakeTorsoParts.Count == 0)
        {
             previousPart = SnakeHead;
            distanceFromParent = 0.02f;
        }
        else
        {
             previousPart = snakeTorsoParts[^1];
        }
            
        newSnakeTorso.transform.SetParent(previousPart.GetTransform());
        previousPart.UnsetLast();
        Vector3 snakeScaleVector = new(snakeScale, snakeScale, snakeScale);
        newSnakeTorso.Setup(moveSpeed, previousPart.GetRotation(), this, snakeScaleVector, distanceFromParent);
        if(SnakeHead.StateMachine.CurrentState == SnakeHead.StateMachine.SpawnedState) newSnakeTorso.SetToTransparent();
        // kopira pozicije, ki so v bufferju od njegovga predhodnika
        if (snakeTorsoParts.Count > 0)
        {
            newSnakeTorso.CopyBuffers(previousPart.GetRotationBuffer(), previousPart.GetPositionBuffer());
        }

        newSnakeTorso.SetPreviousPart(previousPart);
        newSnakeTorso.name = "Torso " + snakeTorsoParts.Count;
        
        snakeTorsoParts.Add(newSnakeTorso);
    }

    float GetAbsoluteRotation(float rotation)
    {
        // get rid of minuses and numbers bigger than 360
        float absoluteMoveRotation = rotation % 360;
        if (absoluteMoveRotation < 0)
        {
            absoluteMoveRotation = 360 + absoluteMoveRotation;
        }
        return absoluteMoveRotation;
    }

    public void GetHit()
    {
        if (snakeTorsoParts.Count - 1 < minTorsoParts)
        {
            // gameOver
        }
        SnakeHeadStateMachine stateMachine = SnakeHead.StateMachine;
        if (stateMachine.CurrentState == stateMachine.SpawnedState) return;

        stateMachine.TransitionTo(stateMachine.SpawnedState);
        
        SnakeTorso lastTorsoPart = snakeTorsoParts.Last();
        snakeTorsoParts.Remove(lastTorsoPart);
        Destroy(lastTorsoPart.gameObject);
    }

    public void HitWall()
    {
        if (snakeTorsoParts.Count - 2 < minTorsoParts)
        {
            // gameOver
        }
        SnakeHeadStateMachine stateMachine = SnakeHead.StateMachine;

        if (stateMachine.CurrentState == stateMachine.BitingState)
        {
            stateMachine.TransitionTo(stateMachine.NormalState);
        }

        blocksToSpawn = snakeTorsoParts.Count - 2;
        SnakeHead.gameObject.SetActive(false);
        foreach (SnakeTorso torso in snakeTorsoParts)
        {
            Destroy(torso.gameObject);
        }
        Path.RemoveMarkers();
        snakeInputManager.OnSnakeDeath();

        PlayerActions.PlayerDeath?.Invoke();

        StartRespawnTimer();
    }

    void StartRespawnTimer()
    {
        timer.SetDuration(waitTime);
    }

    public Vector3 GetSpawnPosition()
    {
        return spawnPosition;
    }

    public void DespawnForNewLevel()
    {
        SnakeHead.RotationBuffer = new LinkedList<float>();
        SnakeHead.gameObject.SetActive(false);
        
        NewLevelSize = snakeTorsoParts.Count;
        for (int i = snakeTorsoParts.Count - 1; i >= 0; i--)
        {
            SnakeTorso torso = snakeTorsoParts[i];
            if (torso != null)
            {
                Destroy(torso.gameObject); // Destroy the GameObject
                snakeTorsoParts.RemoveAt(i); // Remove from the list
            }
        }
    }

    private ISnakeInput CreateInputManager()
    {
        /*
        MobileInputManager mobileInput = new(this);
        return mobileInput;
        
        
        DesktopInputManager desktopInput = new(this);
        return desktopInput;
        */

        
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            MobileInputManager mobileInput = new(this);
            return mobileInput;
        }
        else
        {
            DesktopInputManager desktopInput = new(this);
            return desktopInput;
        }
    }
    public void SetSnakePath(GridObject gridObject, Vector3 location, float rotation)
    {
        Path.SpawnMarker(gridObject, location, rotation);
    }

    public void SetToTransparent()
    {
        foreach (SnakeTorso torso in snakeTorsoParts)
        {
            torso.SetToTransparent();
        }
    }

    public void SetToSolid()
    {
        foreach (SnakeTorso torso in snakeTorsoParts)
        {
            torso.SetToSolid();
        }
    }
}
