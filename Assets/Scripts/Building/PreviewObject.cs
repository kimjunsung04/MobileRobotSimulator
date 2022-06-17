using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    public bool foundation;
    public List<Collider> col = new List<Collider>();
    public Material green;
    public Material red;
    public bool IsBuildable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        changecolor();
    }
    void OnTriggerEnter(Collider other)
    {
        if (col.Count >= 0)
        {
            col = new List<Collider>();
            col.Add(other);
        }
        else
        {
            col.Add(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        col.Remove(other);
    }

    public void changecolor()
    {
        if (col.Count == 0)
        {
            IsBuildable = true;
        }
        else
        {
            IsBuildable = false;
        }

        if (IsBuildable)
        {
            GetComponent<Renderer>().material = green;
        }
        else
        {
            GetComponent<Renderer>().material = red;
        }
    }
}
