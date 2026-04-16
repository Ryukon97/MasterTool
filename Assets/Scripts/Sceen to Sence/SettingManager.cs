using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject Settingpanel;  // 전체 설정창 패널 (ConfigButton)
    public GameObject panelVolume;   // 1. 음량 탭 콘텐츠 (Panel_Volume)
    public GameObject panelOther;    // 2. 기타 탭 콘텐츠 (Panel_other)

    [Header("Brightness")]
    public Image BrightnessOverlay;  // BrightOverlay 이미지
    public Slider BrightnessSlider;  // 화면밝기 슬라이더

    [Header("Sound Setting")]
    public Slider soundSlider;       // BGM 슬라이더
    public Slider sfxSlider;         // 효과음 슬라이더 (SoundEffectSlider)
    public Slider voiceSlider;       // 보이스 슬라이더 (추후용)
    public AudioSource IntroaudioSource;
    public SoundEffect sfxManager;   // SoundEffect 스크립트 연결

    public ChatManager chatManager;

    void Start()
    {
        Debug.Log($"<color=cyan>[Start] 연결 확인 - PanelOther: {panelOther != null}, SettingPanel: {Settingpanel != null}</color>");

        if (panelOther == null) Debug.LogError("마스타! panelOther 변수가 인스펙터에서 비어있습니다!");

        float savedBrightness = PlayerPrefs.GetFloat("SavedBrightness", 0f); //사운드 입력값
        float savedSound = PlayerPrefs.GetFloat("SavedSound", 1.0f);
        float savedSFX = PlayerPrefs.GetFloat("SavedSFX", 1.0f);
        float savedVoice = PlayerPrefs.GetFloat("SavedVoice", 1.0f);

        BrightnessSlider.value = savedBrightness; // 슬라이더 총괄
        soundSlider.value = savedSound;
        if (sfxSlider != null) sfxSlider.value = savedSFX;
        if (voiceSlider != null) voiceSlider.value = savedVoice;

        SetBrightness(savedBrightness); //게임에 적용할것
        Setsound(savedSound);
        SetSFXVolume(savedSFX);

        BrightnessSlider.onValueChanged.AddListener(SetBrightness);
        soundSlider.onValueChanged.AddListener(Setsound);
        if (sfxSlider != null) sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        ShowVolumeTab();
        if (panelVolume != null) panelVolume.SetActive(true);
        if (panelOther != null) panelOther.SetActive(true);
        if (Settingpanel != null) Settingpanel.SetActive(false);
    }

    public void ToggleSettingPanel(bool isActive)
    {
        if (Settingpanel != null) Settingpanel.SetActive(isActive);
    }

    public void ShowVolumeTab() // 옵션창 탭 켰다 껏다하는거
    {
        if (panelVolume != null) panelVolume.SetActive(true);
        if (panelOther != null) panelOther.SetActive(false);
    }

  
     public void ShowOtherTab()
    {
        if (panelVolume != null) panelVolume.SetActive(false); // 음량 슬라이더들 퇴장!
        if (panelOther != null) panelOther.SetActive(true);    // 기타 슬라이더들 등장!

        // 마스타! 만약 레이어 순서 때문에 안 보인다면 아래 줄을 유지하세요.
        panelOther.transform.SetAsLastSibling();

        Debug.Log("Master! 기타 슬라이더로 교체했습니다.");
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

    public void Setsound(float value) //사운드 조절
    {
        AudioListener.volume = value;
    }

    public void SetSFXVolume(float value) //효과음조절
    {
        if (sfxManager != null) sfxManager.SetVolume(value);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("SavedBrightness", BrightnessSlider.value);
        PlayerPrefs.SetFloat("SavedSound", soundSlider.value);
        PlayerPrefs.SetFloat("SavedSFX", sfxSlider.value);
        PlayerPrefs.SetFloat("SavedVoice", voiceSlider.value);
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