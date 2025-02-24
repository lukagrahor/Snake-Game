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
    //LinkedList<float> nextTorsoRotation;
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

    [SerializeField] TMP_Text respawnTimerText;
    [SerializeField] float waitTime = 3f;
    float timer = 0f;
    //float nextTorsoRotation;
    // èe se kaèa obrne ko pobere pickup se odcepi
    void Awake()
    {
        snakeTorsoParts = new List<SnakeTorso>();
        // arena je na poziciji 0, kocka arene je velika 1, kar pomeni da gre za 0.5 gor od 0, kocka od kaèe pa je velika 0.5 --> 0.25
        spawnPosition = new Vector3(0f, arenaBlock.GetBlockSize()/2f + snakeScale/2f, -2f);
        snakeHead = Instantiate(snakeHeadPrefab.gameObject, spawnPosition, Quaternion.identity).GetComponent<SnakeHead>();

        snakeHead.Setup(moveSpeed, (float)startingRotation, this, new Vector3(snakeScale, snakeScale, snakeScale));
    }

    void Start()
    {
        Application.targetFrameRate = 30;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0f)
        {
            RespawnCountdown();
        }
    }

    void RespawnCountdown() // naredi timer interface in pol razliène timerje
    {
        timer -= Time.deltaTime;
        respawnTimerText.text = Math.Ceiling(timer).ToString();
        if (timer < 0f)
        {
            Respawn();
        }
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
        respawnTimerText.text = "";
    }

    public float GetSnakeYRotation()
    {
        return snakeHead.GetRotation();
    }
    /*
    public void SetSnakeYRotation(float turnRotation)
    {
        snakeYRotation = turnRotation;
        //SetTorsoRotation();
        //Debug.Log($"snakeYRotation: {snakeYRotation}");
    }*/
    public void SetNextYRotation(float turnRotation)
    {
        snakeHead.AddToRotationBuffer(turnRotation);
        // Že tle ne dobim ta prave rotacije

        //SetNextTorsoRotation(turnRotation);
        //showNextTorsoRotations();
        //SetTorsoRotation();
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

    // when the head reaches the position of the block it gives the position to the torso partsž
    public void SetTorsoRotation(float nextTorsoRotation)
    {
        //Debug.Log("snakeTorsoParts Count");
        //Debug.Log(snakeTorsoParts.Count);
        if (snakeTorsoParts.Count == 0)
        {
            return;
        }
        //Debug.Log($"nextTorsoRotation {nextTorsoRotation.First.Value}"); // error object reference not set to instance of an object !!!!!!!!!!!!!!!!!!!!!!!!!! --> DOUBLE TAP OB TEM KO POBEREŠ KOCKO
        //Debug.Log($"position of the rotation {snakeHead.transform.position}");
        // nextTorsoRotation ni ta prav, 2x pride isti
        foreach (SnakeTorso torso in snakeTorsoParts)
        {
            // bug --> nextTorsoRotation je null
            //Debug.Log($"nextTorsoRotation: {nextTorsoRotation}");
            torso.AddToRotationBuffer(nextTorsoRotation);
            //Debug.Log($"snakeHead: {snakeHead.transform.position}");
            torso.AddToPositionBuffer(snakeHead.transform.position);

        }

        //nextTorsoRotation.RemoveFirst();
        //snakeTorsoParts[0].PrepareForTurn(snakeHead.transform.position, turnRotation);

        //snakeCorner = Instantiate(snakeCornerPrefab.gameObject).GetComponent<SnakeCorner>();
        //snakeCorner.Setup(snakeHead.transform);
    }
    /*
    public void SetTorsoRotation()
    {
        //Debug.Log("snakeTorsoParts Count");
        //Debug.Log(snakeTorsoParts.Count);
        if (snakeTorsoParts.Count == 0)
        {
            return;
        }
        //Debug.Log($"nextTorsoRotation {nextTorsoRotation.First.Value}"); // error object reference not set to instance of an object !!!!!!!!!!!!!!!!!!!!!!!!!! --> DOUBLE TAP OB TEM KO POBEREŠ KOCKO
        //Debug.Log($"position of the rotation {snakeHead.transform.position}");
        // nextTorsoRotation ni ta prav, 2x pride isti
        foreach (SnakeTorso torso in snakeTorsoParts)
        {
            // bug --> nextTorsoRotation je null
            Debug.Log($"nextTorsoRotation: {nextTorsoRotation.First.Value}");
            torso.AddToRotationBuffer(nextTorsoRotation.First.Value);
            Debug.Log($"snakeHead: {snakeHead.transform.position}");
            torso.AddToPositionBuffer(snakeHead.transform.position);
            
        }

        nextTorsoRotation.RemoveFirst();
        //snakeTorsoParts[0].PrepareForTurn(snakeHead.transform.position, turnRotation);

        //snakeCorner = Instantiate(snakeCornerPrefab.gameObject).GetComponent<SnakeCorner>();
        //snakeCorner.Setup(snakeHead.transform);
    }*/

    public void Grow()
    {
        SnakeTorso newSnakeTorso = Instantiate(snakeTorsoPrefab.gameObject).GetComponent<SnakeTorso>();
        if(snakeTorsoParts.Count == 0)
        {
            newSnakeTorso.transform.SetParent(snakeHead.transform);
            snakeHead.unsetLast();
            newSnakeTorso.Setup(moveSpeed, snakeHead.GetRotation(), transform);
            newSnakeTorso.SetPreviousPart(snakeHead);
            newSnakeTorso.name = "0";
        }
        else
        {
            // trigger za bug je, da se zgodi turn takoj pred tem ko kaèa poje hrano
            ISnakePart previousPart = snakeTorsoParts[snakeTorsoParts.Count - 1];
            newSnakeTorso.transform.SetParent(previousPart.getTransform());
            previousPart.unsetLast();
            float previousTorsoRotation = previousPart.GetRotation();
            //Debug.Log($"currentTorsoRotation: {previousTorsoRotation}");

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
    /*
    public void SetNextTorsoRotation(float nextTorsoRotation)
    {
        // bil je bug kjer se je dodajalo rotacije v buffer, ko ni blo še nobenega torso dela in pole ko je kaèa pobrala kos
        // je upoštevalo vse ta stare rotacije
        if (snakeTorsoParts.Count == 0 || this.nextTorsoRotation.Count == 2) // to prevent spam --> to je bil fix za odcep repa
        {
            return;
        }

        this.nextTorsoRotation.AddLast(nextTorsoRotation);
    }*/
    /*
    void showNextTorsoRotations()
    {
        int j = 0;
        foreach (float rotat in nextTorsoRotation)
        {
            Debug.Log($"Next torso rotation {j}: {rotat}");
            j++;
        }
    }*/

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
        timer = waitTime;
    }

    public Vector3 GetSpawnPosition()
    {
        return spawnPosition;
    }
}
