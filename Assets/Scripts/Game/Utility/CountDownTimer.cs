using System;
using TMPro;
using UnityEngine;

public class CountDownTimer : MonoBehaviour
{
    float timer = 0f;
    bool isRunning = false;
    public event Action TimeRanOut;
    [SerializeField] TMP_Text textField;

    public void SetDuration(float duration)
    {
        timer = duration;
        isRunning = true;

        Debug.Log($"CountDownTimer: Starting timer with duration {duration}s");
        Debug.Log($"CountDownTimer: Event subscribers count: {TimeRanOut?.GetInvocationList()?.Length ?? 0}");
    }

    void Update()
    {
        if (isRunning == true)
        {
            CountDown();
        }
    }

    void CountDown()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            textField.text = Math.Ceiling(timer).ToString();
        } 
        else
        {
            isRunning = false;
            textField.text = "";
            try
            {
                TimeRanOut?.Invoke();
                Debug.Log("CountDownTimer: TimeRanOut event invoked successfully");
            }
            catch (Exception e)
            {
                Debug.LogError($"CountDownTimer: Error invoking TimeRanOut event: {e.Message}");
            }
        }
    }

    void OnDestroy()
    {
        if (isRunning)
        {
            Debug.LogWarning("CountDownTimer: Timer was destroyed while still running!");
        }
    }
}
