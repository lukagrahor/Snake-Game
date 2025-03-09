using UnityEngine;

public class Timer
{
    float startTime = 0f;
    public Timer()
    {

    }

    public void StartTimer()
    {
        startTime = Time.realtimeSinceStartup;
    }
    public float StopTimer()
    {
        return Time.realtimeSinceStartup - startTime;
    }
}
