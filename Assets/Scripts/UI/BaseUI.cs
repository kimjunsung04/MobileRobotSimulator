using Rito;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;


public class BaseUI : MonoBehaviour
{
    public CanvasGroup Popup;
    public Text PopupTitle;
    public Text PopupContent;

    private float fadeTime = 1f;
    private float accumTime = 0f;
    private Coroutine fadeCor;
    private static int nowshow; // 현재 보여주고있는 show id

    public IEnumerator PopupShow(string title, string content, float delay)
    {
        nowshow = UnityEngine.Random.Range(10000000, 99999999); // show id 생성
        int nowcash = nowshow;
        PopContentEdit(title, content);
        StartFadeIn();
        yield return new WaitForSeconds(5);
        StartFadeOut(nowcash);
    }

    public void PopContentEdit(string title, string content)
    {
        PopupTitle.text = title;
        PopupContent.text = content;
    }

    public void StartFadeIn()
    {
        if (fadeCor != null)
        {
            StopAllCoroutines();
            fadeCor = null;
        }
        fadeCor = StartCoroutine(FadeIn());
    }

    public void StartFadeOut(int showid)
    {
        if(showid != nowshow){ // 아이디 다를시 닫지않음(다른팝업 구분)
            return;
        }
        if (fadeCor != null)
        {
            StopAllCoroutines();
            fadeCor = null;
        }
        fadeCor = StartCoroutine(FadeOut());

    }

    private IEnumerator FadeIn()
    {
        accumTime = 0f;
        while (accumTime < fadeTime)
        {
            Popup.alpha = Mathf.Lerp(0f, 1f, accumTime / fadeTime);
            yield return 0;
            accumTime += Time.deltaTime;
        }
        Popup.alpha = 1f;
    }


    private IEnumerator FadeOut()
    {
        accumTime = 0f;
        while (accumTime < fadeTime)
        {
            Popup.alpha = Mathf.Lerp(1f, 0f, accumTime / fadeTime);
            yield return 0;
            accumTime += Time.deltaTime;
        }
        Popup.alpha = 0f;
    }
}
