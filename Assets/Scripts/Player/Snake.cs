using System;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UIElements;

public class Snake : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] SnakeHead snakeHeadPrefab;
    [SerializeField] SnakeTorso snakeTorsoPrefab;
    //[SerializeField] SnakeCorner snakeCornerPrefab;
    SnakeHead snakeHead;
    List<SnakeTorso> snakeTorsoParts;
    //SnakeCorner snakeCorner;

    [SerializeField] ArenaBlock arenaBlock;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float snakeScale = 0.4f;
    Vector3 spawnPosition;

    [SerializeField] float waitTime = 3f;
    [SerializeField] CountDownTimer timer;
    ISnakeInput snakeInputManager;
    void Awake()
    {
        snakeInputManager = CreateInputManager();
        snakeTorsoParts = new List<SnakeTorso>();
        // arena je na poziciji 0, kocka arene je velika 1, kar pomeni da gre za 0.5 gor od 0, kocka od kaèe pa je velika 0.5 --> 0.25
        spawnPosition = new Vector3(0f, arenaBlock.GetBlockSize()/2f + snakeScale/2f, -2f);
        snakeHead = Instantiate(snakeHeadPrefab.gameObject, spawnPosition, Quaternion.identity).GetComponent<SnakeHead>();
        Vector3 snakeScaleVector = new (snakeScale, snakeScale, snakeScale);

        snakeHead.Setup(moveSpeed, this, snakeScaleVector);
        timer.TimeRanOut += Respawn;
    }

    void Start()
    {
        //Application.targetFrameRate = 30;
    }

    void Respawn()
    {
        snakeTorsoParts = new List<SnakeTorso>();
        snakeHead.transform.position = spawnPosition;
        snakeHead.gameObject.SetActive(true);
        snakeInputManager.OnSnakeRespawn();
    }

    public float GetSnakeYRotation()
    {
        return snakeHead.GetRotation();
    }

    public void SetNextYRotation(float turnRotation)
    {
        snakeHead.AddToRotationBuffer(turnRotation);
    }

    /**
     * <summary>Returns what will be the snake head's rotation after it rotates by
     * the next rotation that is saved in the buffer</summary>
     * **/
    public float GetNextHeadRotation()
    {
        if (snakeHead == null)
        {
            return -2f;
        }
        float currentRotation = snakeHead.GetRotation();
        float nextRotation = currentRotation + snakeHead.GetNextRotation();
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
            torso.AddToPositionBuffer(snakeHead.transform.position);
        }
    }

    public void Grow()
    {
        SnakeTorso newSnakeTorso = Instantiate(snakeTorsoPrefab.gameObject).GetComponent<SnakeTorso>();
        ISnakePart previousPart;
        if (snakeTorsoParts.Count == 0)
        {
             previousPart = snakeHead;
        }
        else
        {
             previousPart = snakeTorsoParts[^1];
        }
            
        newSnakeTorso.transform.SetParent(previousPart.GetTransform());
        previousPart.UnsetLast();
        newSnakeTorso.Setup(moveSpeed, previousPart.GetRotation(), transform);

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
        snakeHead.gameObject.SetActive(false);
        foreach (SnakeTorso torso in snakeTorsoParts)
        {
            Destroy(torso.gameObject);
        }
        snakeInputManager.OnSnakeDeath();
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

    private ISnakeInput CreateInputManager()
    {
        /*
        MobileInputManager mobileInput = new(this);
        return mobileInput;
        */
        
        DesktopInputManager desktopInput = new(this);
        return desktopInput;
        
        /*
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
        */
    }
}
