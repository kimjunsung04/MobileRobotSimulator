using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BuildingSystem : MonoBehaviour
{
    public List<buildObjects> objects = new List<buildObjects>();
    public buildObjects currentobject;
    private Vector3 currentpos;
    public Transform currentpreview;
    private GameObject PreviewObject;
    public Transform cam;
    public RaycastHit hit;
    public LayerMask layer;
    public LayerMask Blayer; // ��ġ�� �� ���̾�
    public List<GameObject> Bonmap = new List<GameObject>(); // ���� ����Ʈ
    /* Ÿ�� ���͸��� */
    public Material OrgTile; 
    public Material OrgBanTile;
    public Material GridTile;
    public Material GridBanTile;
    /* ============  */
    public BaseUI BaseUI; // �˾���
    public RawImage MapSavePopup; // â �ݱ� ������

    public float offset = 1.0f;
    public float gridSize = 1.0f;

    public BuildData BuildData = new BuildData(); // �� ��ġ ������
    public List<GameObject> BuildList = new List<GameObject>();

    public bool IsBuilding;
    private bool rmode; // ȸ�����
    private static bool RemoveMode = false; // ���찳 ���


    // Start is called before the first frame update
    void Start()
    {
        currentobject = objects[0]; // ������Ʈ �⺻����
        ChangeCurrentBuilding(); // ������Ʈ ���� ����

        Cursor.visible = false; // Ŀ�� �Ⱥ��̰�
        PreviewObject.SetActive(IsBuilding); // ������ ������Ʈ �����
        TileGridMode(false); // �׸����� ����
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)&&!RemoveMode) // ũ����Ʈ ���
        {
            IsBuilding = IsBuilding ? false : true;
            PreviewObject.SetActive(IsBuilding);
            TileGridMode(IsBuilding);
            if (IsBuilding)
            {
                StartCoroutine(BaseUI.PopupShow("�˸�", "ũ����Ʈ ��� Ȱ��ȭ!", 5));
            }
            else
            {
                StartCoroutine(BaseUI.PopupShow("�˸�", "ũ����Ʈ ��� ��Ȱ��ȭ!", 5));
            }
        }

        if (Input.GetKey(KeyCode.LeftShift)&& Input.GetKeyDown(KeyCode.R)) // ���찳 ���
        {
            RemoveMode = RemoveMode ? false : true;
            if (RemoveMode)
            {
                PreviewObject.SetActive(false); // ������ ������Ʈ �����
                IsBuilding = false; // ũ����Ʈ ��� ��Ȱ��ȭ
                StartCoroutine(BaseUI.PopupShow("�˸�", "���찳 ��� Ȱ��ȭ!", 5));
            }
            else
            {
                StartCoroutine(BaseUI.PopupShow("�˸�", "���찳 ��� ��Ȱ��ȭ!", 5));
            }
        }
        if (RemoveMode) // ���찳 ���
        {
            Remove();
        }
        else if (IsBuilding)
        {
            startPreview();

            if (Input.GetMouseButtonDown(0))
            {
                Build();
            }
            if (Input.GetKeyDown(KeyCode.R)) // ������Ʈ ȸ��
            {
                if (!rmode)
                {
                    currentpreview.transform.Rotate(0, 90, 0);
                    rmode = true;
                }
                else
                {
                    currentpreview.transform.Rotate(0, -90, 0);
                    rmode = false;
                }
            }
        }
    }

    public void ChangeCurrentBuilding()
    {
        rmode = false;
        GameObject curprev = Instantiate(currentobject.preview, currentpos, Quaternion.identity) as GameObject;
        currentpreview = curprev.transform;
        PreviewObject = curprev;
    }

    public void startPreview()
    {
        if(Physics.Raycast(cam.position,cam.forward,out hit, 10, layer))
        {
            if(hit.transform != this.transform)
            {
                showPreview(hit);
            }
        }
    }

    public void showPreview(RaycastHit hit2)
    {
        currentpos = hit2.point;
        currentpos -= Vector3.one * offset;
        currentpos /= gridSize;
        currentpos = new Vector3(Mathf.Round(currentpos.x), currentpos.y + currentobject.yup, Mathf.Round(currentpos.z));
        currentpos *= gridSize;
        currentpos += Vector3.one * offset;
        currentpreview.position = currentpos;
    }

    public void Build()
    {
        PreviewObject P0 = currentpreview.GetComponent<PreviewObject>();
        if (P0.IsBuildable)
        {
            GameObject creatobj = Instantiate(currentobject.gameobj, currentpos, Quaternion.identity);

            if (rmode) // ȸ����� Ȱ��ȭ ��� ȸ�����·� ����
            {
                creatobj.transform.Rotate(0, 90, 0);
            }
            /* �� ���� ����Ʈ */
            BuildList.Add(creatobj);

            BuildData.gameobj.Add(currentobject.name);
            BuildData.currentpos.Add(currentpos);
            BuildData.identity.Add(Quaternion.identity);
            BuildData.rotate.Add(rmode);
            /* �� ���� ����Ʈ */
        }
    }

    public void TileGridMode(bool tf)
    {
        if (tf)
        {
            for (int i = 0; i < Bonmap.Count; i++)
            {
                for (int j = 0; j < Bonmap[i].transform.childCount; j++)
                {
                    Transform CashTile = Bonmap[i].transform.GetChild(j);
                    if (CashTile.name.Contains("OrgTile"))
                    {
                        CashTile.GetComponent<Renderer>().material = GridTile;
                    }
                    else if (CashTile.name.Contains("BanTile"))
                    {
                        CashTile.GetComponent<Renderer>().material = GridBanTile;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < Bonmap.Count; i++)
            {
                for (int j = 0; j < Bonmap[i].transform.childCount; j++)
                {
                    Transform CashTile = Bonmap[i].transform.GetChild(j);
                    if (CashTile.name.Contains("OrgTile"))
                    {
                        CashTile.GetComponent<Renderer>().material = OrgTile;
                    }
                    else if (CashTile.name.Contains("BanTile"))
                    {
                        CashTile.GetComponent<Renderer>().material = OrgBanTile;
                    }
                }
            }
        }
    }

    public void Remove()
    {
        if (Physics.Raycast(cam.position, cam.forward, out hit, 10, Blayer))
        {
            if (Input.GetMouseButtonDown(0))
            {
                int cash_index = BuildList.FindIndex(x => x.Equals(hit.transform.gameObject));
                Debug.Log(cash_index);
                /* �迭 ���� ���� */
                BuildData.gameobj.RemoveAt(cash_index);

                BuildData.currentpos.RemoveAt(cash_index);

                BuildData.identity.RemoveAt(cash_index);

                BuildData.rotate.RemoveAt(cash_index);

                BuildList.RemoveAt(cash_index);

                string str = JsonUtility.ToJson(BuildData);
                /*  */

                Destroy(hit.transform.gameObject);
            }
        }
    }

    public void ChangeBuild(int num) // ������ ����
    {
        Destroy(PreviewObject);
        currentobject = objects[num];
        ChangeCurrentBuilding();
        PreviewObject.SetActive(IsBuilding);
    }
    
    public void ClearBuilds() // ��� �� ����
    {
        for(int i=0;i< BuildList.Count; i++)
        {
            Destroy(BuildList[i]);
        }
        BuildList = new List<GameObject>();
        BuildData = new BuildData();
    }

    public void ChangeRemoveMode()
    {
        RemoveMode = RemoveMode?false:true;
    }

    [System.Obsolete]
    public IEnumerator SaveMap(string name)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", name);
        form.AddField("data", JsonUtility.ToJson(BuildData));

        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:8800/savemap", form);
        yield return www.SendWebRequest();

        if(www.downloadHandler.text == "ok")
        {
            MapSavePopup.transform.gameObject.SetActive(false);
            StartCoroutine(BaseUI.PopupShow("�� ����Ϸ�", "���������� ���� ����Ǿ����ϴ�!", 5));
        }
        else
        {
            MapSavePopup.transform.gameObject.SetActive(false);
            StartCoroutine(BaseUI.PopupShow("�� �������", "�������� ������¸� Ȯ�� ���ּ���.", 5));
        }
    }
}


[System.Serializable]
public class buildObjects
{
    public string name;
    public GameObject gameobj;
    public GameObject preview;
    public int gold;
    public float yup;
}

[System.Serializable]
public class MapDataFormat
{
    public List<string> name;
    public List<string> time;
    public List<string> data;
}

[System.Serializable]
public class BuildData
{
    public List<string> gameobj = new List<string>();
    public List<Vector3> currentpos = new List<Vector3>();
    public List<Quaternion> identity = new List<Quaternion>();
    public List<bool> rotate = new List<bool>();
}