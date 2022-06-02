using Rito;
using System.Collections;
using System.Collections.Generic;
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


    public void PopupShow(string title, string content, float delay)
    {
        PopContentEdit(title, content);
        StartFadeIn();
        Invoke("StartFadeOut",5);
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

    public void StartFadeOut()
    {
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
