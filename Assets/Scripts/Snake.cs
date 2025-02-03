using System;
using UnityEngine;
using System.Collections.Generic;

public class Snake : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] SnakeHead snakeHeadPrefab;
    [SerializeField] SnakeTorso snakeTorsoPrefab;
    [SerializeField] SnakeCorner snakeCornerPrefab;
    SnakeHead snakeHead;
    List<SnakeTorso> snakeTorsoParts;
    SnakeCorner snakeCorner;
    LinkedList<float> nextTorsoRotation;

    float snakeYRotation = 0;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] ArenaBlock arenaBlock;

    float waitTime = 5f;
    //float nextTorsoRotation;
    void Awake()
    {
        snakeTorsoParts = new List<SnakeTorso>();
        nextTorsoRotation = new LinkedList<float>();
        snakeHead = Instantiate(snakeHeadPrefab.gameObject).GetComponent<SnakeHead>();
        snakeHead.Setup(moveSpeed, snakeYRotation, transform, arenaBlock.GetBlockSize(), this);
        // arena je na poziciji 0, kocka arene je velika 1, kar pomeni da gre za 0.5 gor od 0, kocka od kaèe pa je velika 0.5 --> 0.25
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Respawn()
    {
        Debug.Log("Respawn");
        float timer = waitTime;
        while(timer > 0f)
        {
            timer -= Time.deltaTime;
            Debug.Log(timer);
        }
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

        setNextTorsoRotation(turnRotation);
        //showNextTorsoRotations();
        //SetTorsoRotation();
    }

    // when the head reaches the position of the block it gives the position to the torso parts
    public void SetTorsoRotation()
    {
        if (snakeTorsoParts.Count == 0)
        {
            return;
        }
        Debug.Log($"nextTorsoRotation {nextTorsoRotation.First.Value}");
        //Debug.Log($"position of the rotation {snakeHead.transform.position}");
        // nextTorsoRotation ni ta prav, 2x pride isti
        foreach (SnakeTorso torso in snakeTorsoParts)
        {
            torso.AddToRotationBuffer(nextTorsoRotation.First.Value);
            torso.AddToPositionBuffer(snakeHead.transform.position);
        }

        nextTorsoRotation.RemoveFirst();
        //snakeTorsoParts[0].PrepareForTurn(snakeHead.transform.position, turnRotation);

        //snakeCorner = Instantiate(snakeCornerPrefab.gameObject).GetComponent<SnakeCorner>();
        //snakeCorner.Setup(snakeHead.transform);
    }

    public void Grow()
    {
        //Debug.Log("Grow!");
        SnakeTorso newSnakeTorso = Instantiate(snakeTorsoPrefab.gameObject).GetComponent<SnakeTorso>();
        //Debug.Log(newSnakePart.name);
        //Debug.Log($"Parenting to: {transform.name}");
        if(snakeTorsoParts.Count == 0)
        {
            newSnakeTorso.transform.SetParent(snakeHead.transform);
            snakeHead.unsetLast();
            newSnakeTorso.Setup(moveSpeed, snakeHead.GetRotation(), transform);
            newSnakeTorso.SetPreviousPart(snakeHead);
        }
        else
        {
            // trigger za bug je, da se zgodi turn takoj pred tem ko kaèa poje hrano
            ISnakePart previousPart = snakeTorsoParts[snakeTorsoParts.Count - 1];
            newSnakeTorso.transform.SetParent(previousPart.getTransform());
            previousPart.unsetLast();
            //Debug.Log("jaja boys");
            float previousTorsoRotation = previousPart.GetRotation();
            Debug.Log($"currentTorsoRotation: {previousTorsoRotation}");

            newSnakeTorso.Setup(moveSpeed, previousTorsoRotation, transform);
            // kopira pozicije, ki so v bufferju od njegovga predhodnika
            newSnakeTorso.copyBuffers(previousPart.GetRotationBuffer(), previousPart.GetPositionBuffer());

            newSnakeTorso.SetPreviousPart(previousPart);
        }
        
        snakeTorsoParts.Add(newSnakeTorso);
    }

    public float GetAbsoluteRotation()
    {
        // get rid of minuses and numbers bigger than 360
        float absoluteMoveRotation = snakeYRotation % 360;
        if (absoluteMoveRotation < 0)
        {
            absoluteMoveRotation = 360 + absoluteMoveRotation;
        }
        return absoluteMoveRotation;
    }

    public void setNextTorsoRotation(float nextTorsoRotation)
    {
        // bil je bug kjer se je dodajalo rotacije v bufferr, ko ni blo še nobenega torso dela in pole ko je kaèa pobrala kos
        // je upoštevalo vse ta stare rotacije
        if (snakeTorsoParts.Count == 0 || this.nextTorsoRotation.Count == 2) // to prevent spam --> to je bil fix za odcep repa
        {
            return;
        }

        this.nextTorsoRotation.AddLast(nextTorsoRotation);
    }

    void showNextTorsoRotations()
    {
        /*
        int i = 0;
        if (positionBuffer.Count == 0)
        {
            //Debug.Log("Praznu!");
            return;
        }
        foreach (Vector3 pos in positionBuffer)
        {
            //Debug.Log($"Toro position {i}: {pos}");
            i++;
        }
        */
        int j = 0;
        foreach (float rotat in nextTorsoRotation)
        {
            Debug.Log($"Next torso rotation {j}: {rotat}");
            j++;
        }
    }

    public void GetHit()
    {
        Destroy(snakeHead.gameObject);
        foreach (SnakeTorso torso in snakeTorsoParts)
        {
            Destroy(torso.gameObject);
        }
        Respawn();
    }
}
