using UnityEngine;
using UnityEngine.UI;

public class IllustManager : MonoBehaviour
{
   
    public static IllustManager Instance;

    [Header("일러스트칸/오브젝트칸")]
    public Image IlustDisplay;

    [Header("일러스트 보관함 (PNG/sprite를 드래그해서 넣기)")]
    public Sprite [] IllustGallery;

    void Awake()
    {
       
        if (Instance == null) Instance = this;
    }

    public void ChangeIllustByIndex(int index)
    {
       
        if (index >= 0 && index < IllustGallery.Length)
        {
            if (IllustGallery[index] != null)
            {
                IlustDisplay.sprite = IllustGallery[index];
                IlustDisplay.gameObject.SetActive(true);
                Debug.Log($"{index}번 일러스트를 출력합니다.");
            }
        }
        else
        {
            Debug.LogWarning($" {index}번 일러스트는 보관함에 없어요!");
        }
    }


    public void HideIllust()
    {
        if (IlustDisplay != null) IlustDisplay.gameObject.SetActive(false);
    }
}