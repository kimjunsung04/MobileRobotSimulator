using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour
{
    public RectTransform scrollbar;
    private RectTransform menubuttonRT;
    public GameObject menubutton;
    public FreeLookCamera MainCamera; // 카메라 비활성화용
    public RawImage MapSavePopup; // 맵 저장하기 팝업
    public RawImage MapLoadPopup; // 맵 불러오기 팝업
    public GridLayoutGroup MapLoadContent;  // 그리드 레이아웃 그룹
    public InputField SaveNameInputField; // 저장이름 입력필드
    public RawImage MapLoadItemTemplate; // 로드 메뉴 템플릿
    public BaseUI BaseUI; // 베이스 ui 토스트 메세지용

    private bool menuopen = false; // 메뉴 오픈여부
    private Text buttontext;
    private BuildingSystem BuildingSystem; // 건설 시스템
    private GameObject cashbse; // 캐시 베이스 템플릿
    private string MapList; // 서버에서 받아온 맵 리스트
    private List<GameObject> MapListUI = new List<GameObject>(); // 추가된 리스트 ui

    private void Start()
    {
        menubuttonRT = menubutton.GetComponent<RectTransform>();
        buttontext = menubutton.transform.GetChild(0).GetComponent<Text>();
        BuildingSystem = GameObject.Find("MainCamera").GetComponent<BuildingSystem>();
    }
    public void MapSaveButton() // 맵 저장 선택
    {
        MainCamera.MouseShow(false);
        MapSavePopup.transform.gameObject.SetActive(true);
    }

    [System.Obsolete]
    public void MapSaveStartButton() // 맵 저장 전송
    {
        StartCoroutine(BuildingSystem.SaveMap(SaveNameInputField.text));
    }

    public void MapLoadButton() // 맵 불러오기 선택
    {
        MainCamera.MouseShow(false);
        MapLoadPopup.transform.gameObject.SetActive(true);

        StartCoroutine(MapListGet());
    }

    public IEnumerator MapListGet()
    {
        long rcode;
        using (UnityWebRequest request = UnityWebRequest.Get("http://127.0.0.1:8800/loadmap"))
        {
            yield return request.SendWebRequest();
            rcode = request.responseCode;
            if (rcode == 200)
            {
                MapList = request.downloadHandler.text;
            }
            else
            {
                StartCoroutine(BaseUI.PopupShow("맵 로드 실패", "서버와의 연결상태를 확인 해주세요.", 5));
            }
        }
        if (MapListUI.Count != 0) // 로드된 아이템이 0개가 아니면 UI 모두 비우고 다시설정
        {
            for(int i=0;i< MapListUI.Count; i++)
            {
                Destroy(MapListUI[i]);
            }
        }
        if (rcode == 200)
        {
            MapDataFormat OrgCashBuildData = JsonUtility.FromJson<MapDataFormat>(MapList);

            for (int i = 0; i < OrgCashBuildData.data.Count; i++)
            {
                Transform newItem = Instantiate(MapLoadItemTemplate).transform;
                MapListUI.Add(newItem.transform.gameObject);
                /* 포지션 잡기 */
                newItem.transform.parent = MapLoadContent.transform;
                newItem.transform.localScale = Vector3.one;
                /* 포지션 잡기 */
                newItem.transform.gameObject.SetActive(true); // 보이게
                newItem.transform.Find("Name").GetComponent<Text>().text = $"이름 : {OrgCashBuildData.name[i]}";
                newItem.transform.Find("Time").GetComponent<Text>().text = $"생성일 : {OrgCashBuildData.time[i]}";
                int index = i;
                newItem.transform.Find("LoadButton").GetComponent<Button>().onClick.AddListener(() => MapLoadStartButton(OrgCashBuildData.data[index]));
            }
        }
        StartCoroutine(BaseUI.PopupShow("맵 로드 성공", "성공적으로 맵 목록을 불러왔습니다.", 5));

    }

    public void MapLoadStartButton(string datastr)
    {
        GameObject CashObj = null;
        BuildData data = JsonUtility.FromJson<BuildData>(datastr);
        BuildingSystem.ClearBuilds();
        for (int i = 0; i < data.gameobj.Count; i++)
        {
            if (data.gameobj[i] == "LongWall")
            {
                CashObj = BuildingSystem.objects[0].gameobj;
            }
            else if(data.gameobj[i] == "ShortWall")
            {
                CashObj = BuildingSystem.objects[1].gameobj;
            }
            else if(data.gameobj[i] == "PuckStand")
            {
                CashObj = BuildingSystem.objects[2].gameobj;
            }
            GameObject creatobj = Instantiate(CashObj, data.currentpos[i], data.identity[i]);
            if (data.rotate[i]) // 맵 데이터에서 회전처리 되어있다면 회전시키기
            {
                creatobj.transform.Rotate(0, 90, 0);
            }
            BuildingSystem.BuildList.Add(creatobj);

            BuildingSystem.BuildData.gameobj.Add(data.gameobj[i]);
            BuildingSystem.BuildData.currentpos.Add(data.currentpos[i]);
            BuildingSystem.BuildData.identity.Add(data.identity[i]);
            BuildingSystem.BuildData.rotate.Add(data.rotate[i]);

        }
    }

    public void AllClearButton()
    {
        BuildingSystem.ClearBuilds();
        StartCoroutine(BaseUI.PopupShow("알림", "모든 오브젝트 제거!", 5));
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
