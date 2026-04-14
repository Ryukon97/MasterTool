using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{

    [Header("UI Panels")] //기본 UI틀
    public GameObject Settingpanel;

    [Header("Brightness")] //밝기설정
    public Image BrightnessOverlay;
    public Slider BrightnessSlider;

    [Header("Sound")] //사운드바
    public Slider soundSlider;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Settingpanel.SetActive(false);
        BrightnessSlider.value = 0f;
        soundSlider.value = AudioListener.volume;

        BrightnessSlider.onValueChanged.AddListener(SetBrightness);
        soundSlider.onValueChanged.AddListener(Setsound);
    }

    public void ToggleSettingPanel(bool isActive)
    {
        Settingpanel.SetActive(isActive);
    }

    public void SetBrightness(float value)
    {
        if (BrightnessOverlay != null)
        {
            Color color = BrightnessOverlay.color;
            color.a = value;
            BrightnessOverlay.color = color;
        }
    }
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("SavedBrightness", BrightnessSlider.value);
        PlayerPrefs.SetFloat("SavedSound", soundSlider.value);
        PlayerPrefs.Save();
        Debug.Log("설정값이 저장되었습니다!");
    }

    public void Setsound(float value)
    {
        AudioListener.volume = value;
    }
    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터에서 종료
#else
        Application.Quit(); // 빌드된 게임에서 종료
#endif
    }
}
