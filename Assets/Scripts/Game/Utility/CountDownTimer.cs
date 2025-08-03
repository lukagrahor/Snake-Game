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
            TimeRanOut?.Invoke();
            textField.text = "";
        }
    }
}
