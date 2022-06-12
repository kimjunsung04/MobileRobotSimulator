using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEvent : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("This is plane");
    }
}
