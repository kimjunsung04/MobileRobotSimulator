using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEvent : MonoBehaviour
{
    public Material OrgMat;
    public Material CrashMat;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 7) // 로봇과 충동했을때
        {
            gameObject.GetComponent<Renderer>().material = CrashMat;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 7) // 로봇과 충동끝났을때
        {
            gameObject.GetComponent<Renderer>().material = OrgMat;
        }
    }
}
