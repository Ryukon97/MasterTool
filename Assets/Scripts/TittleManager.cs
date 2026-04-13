using UnityEngine;
using UnityEngine.InputSystem;

public class TittleManager : MonoBehaviour
{
    public GameObject TitlePanel;
    public GameObject menuPanel;

    private bool isTitleActive = true;

    void Start()
    {
        TitlePanel.SetActive(true);
        menuPanel.SetActive(false);

    }
    void Update()
    {
        if(isTitleActive && Keyboard.current.anyKey.wasPressedThisFrame) ///키보드 아무버튼
        {
            ShowMenu();
        }
        if (isTitleActive && (Pointer.current?.press.wasPressedThisFrame ?? false)) //마우스 클릭포함
        {
            ShowMenu();
        }
    }
    void ShowMenu()
    {
        isTitleActive = false;
        TitlePanel.SetActive(false); //타이틀메뉴 끄기
        menuPanel.SetActive(true);//메인메뉴 켜기
    }

    public void OnclickStart()
    {
        Debug.Log("게임을 시작합니다");
    }
    public void OnclickExit()
    {
        Debug.Log("게임을 종료합니다");
        Application.Quit();
    }
}
