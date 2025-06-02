using System;
using UnityEngine;

public class CountDown
{
    float timer = 0f;
    bool isRunning = false;
    public event Action TimeRanOut;
    public CountDown (float timer)
    {
        this.timer = timer;
    }

    public void Start()
    {
        isRunning = true;
    }

    public void Update()
    {
        if (isRunning == true)
        {
            Use();
        }
    }

    void Use()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            isRunning = false;
            TimeRanOut?.Invoke();
        }
    }
}