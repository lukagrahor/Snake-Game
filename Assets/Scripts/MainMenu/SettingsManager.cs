using TMPro;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public TMP_Dropdown graphicsDropdown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ChangeGraphicsQuality()
    {
        int graphicSetting = graphicsDropdown.value;
        Debug.Log("graphicSetting: " + graphicSetting);
        QualitySettings.SetQualityLevel(graphicSetting);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
