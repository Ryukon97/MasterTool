using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class SettingManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject Settingpanel;  // 전체 설정창 패널 (ConfigButton)
    public GameObject panelVolume;   // 1. 음량 탭 콘텐츠 (Panel_Volume)
    public GameObject panelOther;    // 2. 기타 탭 콘텐츠 (Panel_other)
    public bool IswaitingForResumeClick = false; //3. 설정창을 닫을시 바로 시작하는게 아닌 한번더 클릭 후 시작할수 있게한다
    public bool IsPausedByMenu = false;

    //[Header("Content Panels (Only Sliders)")]
    //public GameObject volumeContent;
    //public GameObject otherContent;

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

    void Update()
    {
       
        if (IswaitingForResumeClick)
        {
            if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
            {
                
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    IswaitingForResumeClick = false;
                    Time.timeScale = 1f;
                    Debug.Log("Master! 시간을 다시 흐르게 했습니다!");
                }
            }
            return;
        }

      
        if (IsPausedByMenu)
        {
            return;
        }
    }
    void Start()
    {
        Debug.Log($"<color=cyan>[Start] 연결 확인 - PanelOther: {panelOther != null}, SettingPanel: {Settingpanel != null}</color>");

        if (panelOther == null) Debug.LogError("panelOther 변수가 인스펙터에서 비어있습니다!");

        float savedBrightness = PlayerPrefs.GetFloat("SavedBrightness", 0f); //사운드 입력값
        float savedSound = PlayerPrefs.GetFloat("SavedSound", 1.0f);
        float savedSFX = PlayerPrefs.GetFloat("SavedSFX", 1.0f);
        float savedVoice = PlayerPrefs.GetFloat("SavedVoice", 1.0f);
        if (BrightnessSlider != null)
        {
            BrightnessSlider.value = savedBrightness;
            // 리스너를 추가하기 전에 기존 리스너를 제거 (중복 방지)
            BrightnessSlider.onValueChanged.RemoveAllListeners();
            BrightnessSlider.onValueChanged.AddListener(SetBrightness);
            Debug.Log($"<color=white>[Init] 밝기 슬라이더 값 동기화 완료: {savedBrightness}</color>");
        }

        if (soundSlider != null)
        {
            soundSlider.value = savedSound;
            soundSlider.onValueChanged.RemoveAllListeners();
            soundSlider.onValueChanged.AddListener(Setsound);
            Debug.Log($"<color=white>[Init] 음량 슬라이더 값 동기화 완료: {savedSound}</color>");
        }

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


        if (panelVolume != null) panelVolume.SetActive(false);

        if (panelOther != null) panelOther.SetActive(false);
        if (Settingpanel != null) Settingpanel.SetActive(false);
        ShowVolumeTab();
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


        panelOther.transform.SetAsLastSibling();

        Debug.Log(" 기타 슬라이더로 교체했습니다.");
    }


    public void SetBrightness(float value)
    {
        if (BrightnessOverlay != null)
        {
            Color color = BrightnessOverlay.color;
            color.a = value;
            BrightnessOverlay.color = color;
            Debug.Log($"<color=yellow>[Live Edit] 밝기 변경됨: {value} (Overlay Alpha: {color.a})</color>");
        }
        else
        {
            Debug.LogWarning("[Warning] BrightnessOverlay 오브젝트가 비어있습니다!");
        }
    }

    public void Setsound(float value) //사운드 조절
    {
        AudioListener.volume = value;
        Debug.Log($"<color=lime>[Live Edit] 전체 볼륨 변경됨: {value}</color>");
    }

    public void SetSFXVolume(float value) //효과음조절
    {
        if (sfxManager != null) sfxManager.SetVolume(value);
    }

    public void SaveSettings() //저장버튼 연결되어있음
    {
        PlayerPrefs.SetFloat("SavedBrightness", BrightnessSlider.value);
        PlayerPrefs.SetFloat("SavedSound", soundSlider.value);
        PlayerPrefs.SetFloat("SavedSFX", sfxSlider.value);
        PlayerPrefs.SetFloat("SavedVoice", voiceSlider.value);
        PlayerPrefs.Save();
        ResumeGame();
        
    }

    public void OpenSettingPanel()
    {
        if (Settingpanel != null)
        {
            Settingpanel.SetActive(true);
            if (chatManager != null) chatManager.isPausedByMenu = true;

            Time.timeScale = 0f; // 설정 누르면 게임 멈춤
        }
    }

    public void CloseSettingPanel()
    {
        if (Settingpanel != null)
        {
            //Settingpanel.SetActive(false);
            //if (chatManager != null)
            //{
            //    chatManager.isPausedByMenu = false;
            //    IswaitingForResumeClick = true;
            //}

            Settingpanel.SetActive(false);
            ResumeGame();
        }
    }

    private void ResumeGame()
    {
        if(chatManager != null)
        {
            chatManager.isPausedByMenu = false;
           
        }
        IswaitingForResumeClick = false;
        //Time.timeScale = 1f;
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