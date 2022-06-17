using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotUIView : MonoBehaviour
{

    // ī�޶� ����
    public Camera getCamera;
    public GameObject Target;
    public GameObject UI;
    public Material ColorMatrial;
    public Color startColor;
    public Color mouseOverColor;

    // �����ɽ�Ʈ�� �ǵ帰(?) ���� ����ؼ� �־�δ°�
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
