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

    float snakeYRotation = 0;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] ArenaBlock arenaBlock;
    float nextTorsoRotation;
    void Awake()
    {
        snakeTorsoParts = new List<SnakeTorso>();
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

    public float GetYRotation()
    {
        return snakeYRotation;
    }
    public void SetYRotation(float turnRotation)
    {
        snakeYRotation = turnRotation;
        //Debug.Log($"snakeYRotation: {snakeYRotation}");
    }
    public void SetNextYRotation(float turnRotation)
    {
        snakeHead.AddToRotationBuffer(turnRotation);
        // Že tle ne dobim ta prave rotacije
        nextTorsoRotation = turnRotation;
        //SetTorsoRotation(turnRotation);
    }

    // when the head reaches the position of the block it gives the position to the torso parts
    public void SetTorsoRotation()
    {
        if (snakeTorsoParts.Count == 0)
        {
            return;
        }
        //Debug.Log("Uspelo mi je juhej");
        foreach (SnakeTorso torso in snakeTorsoParts)
        {
            torso.AddToRotationBuffer(nextTorsoRotation);
            torso.AddToPositionBuffer(snakeHead.transform.position);
        }
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
            newSnakeTorso.Setup(moveSpeed, snakeYRotation, transform);
        }
        else
        {
            ISnakePart nextPart = snakeTorsoParts[snakeTorsoParts.Count - 1];
            newSnakeTorso.transform.SetParent(nextPart.getTransform());
            nextPart.unsetLast();
            //Debug.Log("jaja boys");
            newSnakeTorso.Setup(moveSpeed, snakeYRotation, transform);
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
}
