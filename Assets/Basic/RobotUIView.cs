using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotUIView : MonoBehaviour
{

    // 카메라를 지정
    public Camera getCamera;
    public GameObject Target;
    public GameObject UI;

    // 레이케스트가 건드린(?) 것을 취득해서 넣어두는곳
    private RaycastHit hit;

    // Update is called once per frame
    void Update()
    {
        // 마우스 클릭을 하면~
        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 포지션을 취득해서 대입해
            Ray ray = getCamera.ScreenPointToRay(Input.mousePosition);

            // 마우스 포지션에서 레이를 던져서 뭔가가 걸리면 hit에 넣음
            if (Physics.Raycast(ray, out hit))
            {
                // 오브젝트명을 취득해서 변수에 넣음
                string objectName = hit.collider.gameObject.name;
                // 오브젝트명을 콘솔에 표시해줌
                if (objectName == Target.name)
                {
                    UI.SetActive(true);
                }
            }
        }
    }
}
