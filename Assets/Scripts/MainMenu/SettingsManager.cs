using TMPro;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public TMP_Dropdown graphicsDropdown;

    public void ChangeGraphicsQuality()
    {
        /*if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
        }*/
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
