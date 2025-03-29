using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    public TMP_Text fpsText;
    private float deltaTime = 0.0f;
    void Start()
    {
    #if UNITY_ANDROID || UNITY_IOS
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 30;
    #elif UNITY_STANDALONE || UNITY_EDITOR
        //QualitySettings.vSyncCount = 0;
        //pplication.targetFrameRate = 30;
    #endif
    }
    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = "FPS: " + Mathf.Ceil(fps).ToString();
    }
}