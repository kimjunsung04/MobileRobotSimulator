using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public LayerMask Blayer; // 설치된 블럭 레이어
    public List<GameObject> Bonmap = new List<GameObject>(); // 본맵 리스트
    /* 타일 머터리얼 */
    public Material OrgTile; 
    public Material OrgBanTile;
    public Material GridTile;
    public Material GridBanTile;
    /* ============  */
    public BaseUI BaseUI;

    public float offset = 1.0f;
    public float gridSize = 1.0f;

    public bool IsBuilding;
    private bool rmode; // 회전모드
    private static bool RemoveMode = false; // 지우개 모드


    // Start is called before the first frame update
    void Start()
    {
        currentobject = objects[0]; // 오브젝트 기본지정
        ChangeCurrentBuilding(); // 오브젝트 변경 적용

        Cursor.visible = false; // 커서 안보이게
        PreviewObject.SetActive(IsBuilding); // 프리뷰 오브젝트 숨기기
        TileGridMode(false); // 그리드모드 오프
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)&&!RemoveMode) // 크래프트 모드
        {
            IsBuilding = IsBuilding ? false : true;
            PreviewObject.SetActive(IsBuilding);
            TileGridMode(IsBuilding);
            if (IsBuilding)
            {
                StartCoroutine(BaseUI.PopupShow("알림", "크래프트 모드 활성화!", 5));
            }
            else
            {
                StartCoroutine(BaseUI.PopupShow("알림", "크래프트 모드 비활성화!", 5));
            }
        }

        if (Input.GetKey(KeyCode.LeftShift)&& Input.GetKeyDown(KeyCode.R)) // 지우개 모드
        {
            RemoveMode = RemoveMode ? false : true;
            if (RemoveMode)
            {
                PreviewObject.SetActive(false); // 프리뷰 오브젝트 숨기기
                IsBuilding = false; // 크래프트 모드 비활성화
                StartCoroutine(BaseUI.PopupShow("알림", "지우개 모드 활성화!", 5));
            }
            else
            {
                StartCoroutine(BaseUI.PopupShow("알림", "지우개 모드 비활성화!", 5));
            }
        }
        if (RemoveMode) // 지우개 모드
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
            if (Input.GetKeyDown(KeyCode.R)) // 오브젝트 회전
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
            GameObject creatobj = Instantiate(currentobject.prefab, currentpos, Quaternion.identity);
            if (rmode)
            {
                creatobj.transform.Rotate(0, 90, 0);
            }
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
                Destroy(hit.transform.gameObject);
            }
        }
    }

    public void ChangeBuild(int num)
    {
        Destroy(PreviewObject);
        currentobject = objects[num];
        ChangeCurrentBuilding();
        PreviewObject.SetActive(IsBuilding);
    }

    public void ChangeRemoveMode()
    {
        RemoveMode = RemoveMode?false:true;
    }
}


[System.Serializable]
public class buildObjects
{
    public string name;
    public GameObject prefab;
    public GameObject preview;
    public int gold;
    public float yup;
}
