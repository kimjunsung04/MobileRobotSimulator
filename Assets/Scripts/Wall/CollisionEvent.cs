using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEvent : MonoBehaviour
{
    public Material OrgMat;
    public Material CrashMat;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 7) // �κ��� �浿������
        {
            gameObject.GetComponent<Renderer>().material = CrashMat;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 7) // �κ��� �浿��������
        {
            gameObject.GetComponent<Renderer>().material = OrgMat;
        }
    }
}
