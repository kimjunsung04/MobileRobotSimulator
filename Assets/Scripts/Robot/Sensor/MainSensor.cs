using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSensor : MonoBehaviour
{
    public Text PSensorView;
    public LineRenderer p0;
    public LineRenderer p1;
    public LineRenderer p2;
    public LineRenderer p3;
    public LineRenderer p4;
    public LineRenderer p5;
    public LineRenderer p6;
    public LineRenderer p7;
    public LineRenderer p8;

    public static float[] p = new float[9];
    Color basC; // 기본 색
    Color warC; // 경고 색
    Color danC; // 위험 색

    RaycastHit hit;

    void Start()
    {
        ColorUtility.TryParseHtmlString("#00FF1399", out basC);
        ColorUtility.TryParseHtmlString("#FFB10099", out warC);
        ColorUtility.TryParseHtmlString("#FF000799", out danC);
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        LineRenderer[] Sensors = { p0, p1, p2, p3, p4, p5, p6, p7, p8 };
        for (int i = 0; i < 9; i++)
        {
            if (Sensors[i])
            {
                Sensors[i].SetPosition(0, Sensors[i].transform.position);
                if (Physics.Raycast(Sensors[i].transform.position, Sensors[i].transform.forward, out hit))
                {
                    if (hit.collider)
                    {
                        Sensors[i].SetPosition(1, hit.point);
                    }
                }
                else Sensors[i].SetPosition(1, Sensors[i].transform.forward * 5000);
                p[i] = hit.distance;
                Debug.Log($"{i}번센서: {p[i]}");
                Debug.DrawRay(Sensors[i].transform.position, Sensors[i].transform.forward*hit.distance, Color.red);
            }
        }
        for (int i = 0; i < 9; i++)
        {
            Transform pm = PSensorView.transform.GetChild(i);
            RawImage pimg = PSensorView.transform.Find($"p{i}").GetComponent<RawImage>();
            Text ptext = pm.transform.Find($"p{i}Text").GetComponent<Text>();
            if (p[i] < 0.2f)
            {
                pimg.color = danC;
            }
            else if(p[i] < 0.6f)
            {
                pimg.color = warC;
            }
            else
            {
                pimg.color = basC;
            }
            ptext.text = $"{p[i]}";
        }
    }
}