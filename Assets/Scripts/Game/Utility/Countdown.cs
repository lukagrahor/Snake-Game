using System;
using UnityEngine;

public class CountDown
{
    float timer = 0f;
    bool isRunning = false;
    public event Action TimeRanOut;
    public float Timer { get => timer; set => timer = value; }
    public CountDown (float timer)
    {
        this.timer = timer;
    }

    public void Start()
    {
        isRunning = true;
    }

    public void Stop()
    {
        isRunning = false;
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