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
    enum Directions {
        Up = 0,
        Right = 90,
        Down = 180,
        Left = 270
    }

    [SerializeField] Directions startingRotation = Directions.Up;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] ArenaBlock arenaBlock;
    [SerializeField] float snakeScale = 0.4f;
    Vector3 spawnPosition;

    //[SerializeField] TMP_Text respawnTimerText;
    [SerializeField] float waitTime = 3f;
    //float timer = 0f;
    [SerializeField] CountDownTimer timer;
    void Awake()
    {
        snakeTorsoParts = new List<SnakeTorso>();
        // arena je na poziciji 0, kocka arene je velika 1, kar pomeni da gre za 0.5 gor od 0, kocka od kaèe pa je velika 0.5 --> 0.25
        spawnPosition = new Vector3(0f, arenaBlock.GetBlockSize()/2f + snakeScale/2f, -2f);
        snakeHead = Instantiate(snakeHeadPrefab.gameObject, spawnPosition, Quaternion.identity).GetComponent<SnakeHead>();

        snakeHead.Setup(moveSpeed, (float)startingRotation, this, new Vector3(snakeScale, snakeScale, snakeScale));
        timer.TimeRanOut += Respawn;
    }

    void Start()
    {
        //Application.targetFrameRate = 30;
    }

    void Update()
    {

    }

    void Respawn()
    {
        Debug.Log("Respawn");
        transform.position = Vector3.zero;
        snakeTorsoParts = new List<SnakeTorso>();
        //nextTorsoRotation = new LinkedList<float>();

        snakeHead = Instantiate(snakeHeadPrefab.gameObject, spawnPosition, Quaternion.identity).GetComponent<SnakeHead>();
        snakeHead.Setup(moveSpeed, (float)startingRotation, this, new Vector3(snakeScale, snakeScale, snakeScale));
        GetComponent<SnakeMovement>().OnSnakeRespawn();
    }

    public float GetSnakeYRotation()
    {
        return snakeHead.GetRotation();
    }

    public void SetNextYRotation(float turnRotation)
    {
        snakeHead.AddToRotationBuffer(turnRotation);
    }

    public float GetNextHeadRotation()
    {
        if (snakeHead == null)
        {
            return -2f;
        }
        float currentRotation = snakeHead.GetRotation();
        float nextRotation = currentRotation + snakeHead.GetNextRotation();
        Debug.Log($"currentRotation kurentovanje: {currentRotation}");
        Debug.Log($"nextRotation prev fest: {snakeHead.GetNextRotation()}");
        Debug.Log($"nextRotation next fest: {nextRotation}");
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
        if(snakeTorsoParts.Count == 0)
        {
            newSnakeTorso.transform.SetParent(snakeHead.transform);
            snakeHead.UnsetLast();
            newSnakeTorso.Setup(moveSpeed, snakeHead.GetRotation(), transform);
            newSnakeTorso.SetPreviousPart(snakeHead);
            newSnakeTorso.name = "0";
        }
        else
        {
            ISnakePart previousPart = snakeTorsoParts[snakeTorsoParts.Count - 1];
            newSnakeTorso.transform.SetParent(previousPart.GetTransform());
            previousPart.UnsetLast();
            float previousTorsoRotation = previousPart.GetRotation();

            newSnakeTorso.Setup(moveSpeed, previousTorsoRotation, transform);
            // kopira pozicije, ki so v bufferju od njegovga predhodnika
            newSnakeTorso.CopyBuffers(previousPart.GetRotationBuffer(), previousPart.GetPositionBuffer());

            newSnakeTorso.SetPreviousPart(previousPart);
            newSnakeTorso.name = ""+snakeTorsoParts.Count;
        }
        
        snakeTorsoParts.Add(newSnakeTorso);
    }
    
    public float GetAbsoluteRotation(float rotation)
    {
        // get rid of minuses and numbers bigger than 360
        Debug.Log($"rotation: {rotation}");
        float absoluteMoveRotation = rotation % 360;
        if (absoluteMoveRotation < 0)
        {
            absoluteMoveRotation = 360 + absoluteMoveRotation;
        }
        return absoluteMoveRotation;
    }

    public void GetHit()
    {
        Destroy(snakeHead.gameObject);
        foreach (SnakeTorso torso in snakeTorsoParts)
        {
            Destroy(torso.gameObject);
        }
        GetComponent<SnakeMovement>().OnSnakeDeath();
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
}
