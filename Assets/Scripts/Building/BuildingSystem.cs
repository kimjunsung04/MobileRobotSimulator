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

    public float offset = 1.0f;
    public float gridSize = 1.0f;

    public bool IsBuilding;


    // Start is called before the first frame update
    void Start()
    {
        currentobject = objects[0];
        ChangeCurrentBuilding();

        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) // 크래프트 모드
        {
            IsBuilding = IsBuilding ? false : true;
            PreviewObject.SetActive(IsBuilding);
        }
        if (IsBuilding)
        {
            startPreview();

            if (Input.GetMouseButtonDown(0))
            {
                Build();
            }
            if (Input.GetKeyDown(KeyCode.R)) // 오브젝트 회전
            {
                currentpreview.transform.Rotate(0, 90, 0);
            }
        }
    }

    public void ChangeCurrentBuilding()
    {
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
        currentpos = new Vector3(Mathf.Round(currentpos.x), Mathf.Round(currentpos.y+1.2f), Mathf.Round(currentpos.z));
        currentpos *= gridSize;
        currentpos += Vector3.one * offset;
        currentpreview.position = currentpos;
    }

    public void Build()
    {
        PreviewObject P0 = currentpreview.GetComponent<PreviewObject>();
        if (P0.IsBuildable)
        {
            Instantiate(currentobject.prefab,currentpos, Quaternion.identity);
        }
    }
}


[System.Serializable]
public class buildObjects
{
    public string name;
    public GameObject prefab;
    public GameObject preview;
    public int gold;
}
