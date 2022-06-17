using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotUIView : MonoBehaviour
{

    // 카메라를 지정
    public Camera getCamera;
    public GameObject Target;
    public GameObject UI;
    public Material ColorMatrial;
    public Color startColor;
    public Color mouseOverColor;

    // 레이케스트가 건드린(?) 것을 취득해서 넣어두는곳
    private RaycastHit hit;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(getCamera.transform.position, getCamera.transform.forward, out hit, 8))
            {
                if (hit.collider.gameObject.name == Target.name)
                {
                    ColorMatrial.SetColor("_Color", mouseOverColor);
                    UI.SetActive(true);
                }
            }
        }
        if (Physics.Raycast(getCamera.transform.position, getCamera.transform.forward, out hit, 8))
        {
            if (hit.collider.gameObject.name == Target.name)
            {
                ColorMatrial.SetColor("_Color", mouseOverColor);
            }
            else
            {
                ColorMatrial.SetColor("_Color", startColor);
            }
        }
    }
}
