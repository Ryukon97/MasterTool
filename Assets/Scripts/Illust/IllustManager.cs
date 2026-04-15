using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class IllustData
{
    public string illustKey;
    public Sprite illustSprite; 
}
public class IllustManager : MonoBehaviour
{
   

    public static IllustManager Instance;

    [Header("일러스트칸/하이라이커 Image를 넣으시오")]
    public Image IlustDisplay;

    [Header("일러스트 보관함 (PNG/sprite를 드래그해서 넣기)")]
    public List<IllustData> IllustGallery = new List<IllustData>(); // []배열에서 리스트로 업그레이드

    void Awake()
    {
       
        if (Instance == null) Instance = this;
    }

    public void ChangeIllust(string keyName)
    {
        if (string.IsNullOrEmpty(keyName)) //Json파일에 일러스트이름이 없으면 호출안됨
        {
            HideIllust();
            return;
        }
       
        IllustData data = IllustGallery.Find(x => x.illustKey == keyName);

        if (data != null && data.illustSprite != null)
        {
            IlustDisplay.sprite = data.illustSprite;
            IlustDisplay.gameObject.SetActive(true);
            Debug.Log($"'{keyName}' 일러스트를 출력합니다.");

            Color c = IlustDisplay.color;
            c.a = 1f;
            IlustDisplay.color = c;
        }
        else
        {
            Debug.LogWarning($"보관함에 '{keyName}'라는 이름의 일러스트가 없어요!");
            HideIllust() ;
        }
    }



    public void HideIllust()
    {
        if (IlustDisplay != null) IlustDisplay.gameObject.SetActive(false);
    }
}