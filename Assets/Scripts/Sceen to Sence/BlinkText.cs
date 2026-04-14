using UnityEngine;
using TMPro;
using System.Collections;
using System;
public class BlinkText :MonoBehaviour
{
    private TextMeshProUGUI textComponent;
    public float BlinkSpeed = 1.0f; 

    void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();

        if( textComponent != null )
        {
            StartCoroutine(BlinkRoutine());
        }
        else
        {
            Debug.Log("꼭 텍스트 넣을것!");

        }    
    }

    IEnumerator BlinkRoutine()
    {
        while (true)
        {
           
            float time = 0f;
            while (time < 1.0f)
            {
                time += Time.deltaTime * BlinkSpeed;
               
                SetAlpha(Mathf.Lerp(1f, 0f, time));
                yield return null; 
            }

            time = 0f;
            while (time < 1.0f)
            {
                time += Time.deltaTime * BlinkSpeed;
            
                SetAlpha(Mathf.Lerp(0f, 1f, time));
                yield return null; 
            }
        }
    }
    void SetAlpha(float alpha)
    {
        Color color = textComponent.color;
        color.a = alpha;
        textComponent.color = color;
    }
}
