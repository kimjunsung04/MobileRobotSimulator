using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotUIView : MonoBehaviour
{

    // ī�޶� ����
    public Camera getCamera;
    public GameObject Target;
    public GameObject UI;

    // �����ɽ�Ʈ�� �ǵ帰(?) ���� ����ؼ� �־�δ°�
    private RaycastHit hit;

    // Update is called once per frame
    void Update()
    {
        // ���콺 Ŭ���� �ϸ�~
        if (Input.GetMouseButtonDown(0))
        {
            // ���콺 �������� ����ؼ� ������
            Ray ray = getCamera.ScreenPointToRay(Input.mousePosition);

            // ���콺 �����ǿ��� ���̸� ������ ������ �ɸ��� hit�� ����
            if (Physics.Raycast(ray, out hit))
            {
                // ������Ʈ���� ����ؼ� ������ ����
                string objectName = hit.collider.gameObject.name;
                // ������Ʈ���� �ֿܼ� ǥ������
                if (objectName == Target.name)
                {
                    UI.SetActive(true);
                }
            }
        }
    }
}
