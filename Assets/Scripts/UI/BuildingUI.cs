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
    public FreeLookCamera MainCamera; // ī�޶� ��Ȱ��ȭ��
    public RawImage MapSavePopup; // �� �����ϱ� �˾�
    public RawImage MapLoadPopup; // �� �ҷ����� �˾�
    public GridLayoutGroup MapLoadContent;  // �׸��� ���̾ƿ� �׷�
    public InputField SaveNameInputField; // �����̸� �Է��ʵ�
    public RawImage MapLoadItemTemplate; // �ε� �޴� ���ø�
    public BaseUI BaseUI; // ���̽� ui �佺Ʈ �޼�����

    private bool menuopen = false; // �޴� ���¿���
    private Text buttontext;
    private BuildingSystem BuildingSystem; // �Ǽ� �ý���
    private GameObject cashbse; // ĳ�� ���̽� ���ø�
    private string MapList; // �������� �޾ƿ� �� ����Ʈ
    private List<GameObject> MapListUI = new List<GameObject>(); // �߰��� ����Ʈ ui

    private void Start()
    {
        menubuttonRT = menubutton.GetComponent<RectTransform>();
        buttontext = menubutton.transform.GetChild(0).GetComponent<Text>();
        BuildingSystem = GameObject.Find("MainCamera").GetComponent<BuildingSystem>();
    }
    public void MapSaveButton() // �� ���� ����
    {
        MainCamera.MouseShow(false);
        MapSavePopup.transform.gameObject.SetActive(true);
    }

    [System.Obsolete]
    public void MapSaveStartButton() // �� ���� ����
    {
        StartCoroutine(BuildingSystem.SaveMap(SaveNameInputField.text));
    }

    public void MapLoadButton() // �� �ҷ����� ����
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
                StartCoroutine(BaseUI.PopupShow("�� �ε� ����", "�������� ������¸� Ȯ�� ���ּ���.", 5));
            }
        }
        if (MapListUI.Count != 0) // �ε�� �������� 0���� �ƴϸ� UI ��� ���� �ٽü���
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
                /* ������ ��� */
                newItem.transform.parent = MapLoadContent.transform;
                newItem.transform.localScale = Vector3.one;
                /* ������ ��� */
                newItem.transform.gameObject.SetActive(true); // ���̰�
                newItem.transform.Find("Name").GetComponent<Text>().text = $"�̸� : {OrgCashBuildData.name[i]}";
                newItem.transform.Find("Time").GetComponent<Text>().text = $"������ : {OrgCashBuildData.time[i]}";
                int index = i;
                newItem.transform.Find("LoadButton").GetComponent<Button>().onClick.AddListener(() => MapLoadStartButton(OrgCashBuildData.data[index]));
            }
        }
        StartCoroutine(BaseUI.PopupShow("�� �ε� ����", "���������� �� ����� �ҷ��Խ��ϴ�.", 5));

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
            if (data.rotate[i]) // �� �����Ϳ��� ȸ��ó�� �Ǿ��ִٸ� ȸ����Ű��
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
        StartCoroutine(BaseUI.PopupShow("�˸�", "��� ������Ʈ ����!", 5));
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
        if (menuopen) // �޴� ����������
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
