using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject Settingpanel;

    [Header("Brightness")]
    public Image BrightnessOverlay;
    public Slider BrightnessSlider;

    [Header("Sound Setting")]
    public Slider soundSlider;
    public AudioSource IntroaudioSource;

    public ChatManager chatManager;

    void Start()
    {
        float savedBrightness = PlayerPrefs.GetFloat("SavedBrightness", 0f);
        float savedSound = PlayerPrefs.GetFloat("SavedSound", 1.0f);

        BrightnessSlider.value = savedBrightness;
        soundSlider.value = savedSound;

        SetBrightness(savedBrightness);
        Setsound(savedSound);

        BrightnessSlider.onValueChanged.AddListener(SetBrightness);
        soundSlider.onValueChanged.AddListener(Setsound);

        if (Settingpanel != null) Settingpanel.SetActive(false);
    }

    public void ToggleSettingPanel(bool isActive)
    {
        if (Settingpanel != null) Settingpanel.SetActive(isActive);
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

    public void Setsound(float value)
    {
        AudioListener.volume = value;
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("SavedBrightness", BrightnessSlider.value);
        PlayerPrefs.SetFloat("SavedSound", soundSlider.value);
        PlayerPrefs.Save();
    }

    public void OpenSettingPanel()
    {
        if (Settingpanel != null)
        {
            Settingpanel.SetActive(true);
            if (chatManager != null) chatManager.isPausedByMenu = true;
        }
    }

    public void CloseSettingPanel()
    {
        if (Settingpanel != null)
        {
            Settingpanel.SetActive(false);
            if (chatManager != null) chatManager.isPausedByMenu = false;
        }
    }

    public void StopIntroBGM()
    {
        if (IntroaudioSource != null) IntroaudioSource.Stop();
    }

    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}