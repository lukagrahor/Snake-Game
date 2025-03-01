using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;

public class FPSDisplay : MonoBehaviour
{
    public TMP_Text fpsText;
    private float deltaTime = 0.0f;
    void Start()
    {
    #if UNITY_ANDROID || UNITY_IOS
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
#elif UNITY_STANDALONE || UNITY_EDITOR
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = -1;
#endif
        Debug.Log("Supports R16 SFloat: " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.R16));
        Debug.Log("Supports ARGBHalf: " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf));
        Debug.Log("Supports Default Format: " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Default));
    }
    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = "FPS: " + Mathf.Ceil(fps).ToString();
    }
}