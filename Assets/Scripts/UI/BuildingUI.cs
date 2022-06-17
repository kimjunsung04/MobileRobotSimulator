using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour
{
    public RectTransform scrollbar;
    private RectTransform menubuttonRT;
    public GameObject menubutton;

    private bool menuopen = false; // 메뉴 오픈여부
    private Text buttontext;
    private BuildingSystem BuildingSystem;

    private void Start()
    {
        menubuttonRT = menubutton.GetComponent<RectTransform>();
        buttontext = menubutton.transform.GetChild(0).GetComponent<Text>();
        BuildingSystem = GameObject.Find("MainCamera").GetComponent<BuildingSystem>();
    }

    public void LongWallButton()
    {
        BuildingSystem.ChangeBuild(0);
    }
    public void ShortWallButton()
    {
        BuildingSystem.ChangeBuild(1);
    }
    public void PuckStandButton()
    {
        BuildingSystem.ChangeBuild(2);
    }

    public void RemoveButton()
    {
        BuildingSystem.ChangeRemoveMode();
    }

    public void MenuOpenOnClick()
    {
        if (menuopen) // 메뉴 열려있을때
        {
            menuopen = false;
            buttontext.text = ">";
            StartCoroutine(MenuClose());
        }
        else
        {
            menuopen = true;
            buttontext.text = "<";
            StartCoroutine(MenuOpen());
        }
    }

    private IEnumerator MenuOpen()
    {
        while (scrollbar.anchoredPosition.x <= 150)
        {
            yield return 0;
            scrollbar.anchoredPosition = new Vector3(scrollbar.anchoredPosition.x + 4   , scrollbar.anchoredPosition.y);
            menubuttonRT.anchoredPosition = new Vector3(menubuttonRT.anchoredPosition.x + 4, menubuttonRT.anchoredPosition.y);
        }
    }

    private IEnumerator MenuClose()
    {
        while (scrollbar.anchoredPosition.x >= -150)
        {
            yield return 0;
            scrollbar.anchoredPosition = new Vector3(scrollbar.anchoredPosition.x - 4, scrollbar.anchoredPosition.y);
            menubuttonRT.anchoredPosition = new Vector3(menubuttonRT.anchoredPosition.x - 4, menubuttonRT.anchoredPosition.y);
        }
    }
}
