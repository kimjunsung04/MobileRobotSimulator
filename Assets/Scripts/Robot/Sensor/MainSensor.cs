using System;
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

    public static float[] porg = new float[9];
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
        for (int i = 0; i < 9; i++) // p센서 실시간 로드
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
                float cashdist = (hit.distance * 200) - 255; // 거리값 조정 핵심 
                if(cashdist >= -30) 
                { 
                    cashdist = -30; 
                } 
                float discash = hit.distance;
                int ppval = (int)Math.Abs(cashdist); // p센서 증가값
                if (ppval <= 250 && ppval >= 227) // 256~200영역
                {
                    discash = 200 + (2.434782608695652f * (ppval - 227));
                }
                else if (ppval <= 227 && ppval >= 220) // 200~150영역
                {
                    discash = 150 + (7.142857142857143f * (ppval - 220));
                }
                else if (ppval <= 220 && ppval >= 208) // 150~120영역
                {
                    discash = 120 + (2.5f * (ppval - 208));
                }
                else if (ppval <= 208 && ppval >= 200) // 120~100영역
                {
                    discash = 100 + (2.5f * (ppval - 200));
                }
                else if (ppval <= 200 && ppval >= 190) // 100~80영역
                {
                    discash = 80 + (2 * (ppval - 190));
                }
                else if (ppval <= 190 && ppval >= 163) // 80~60영역
                {
                    discash = 60 + (0.7407407407407407f * (ppval - 163));
                }
                else
                {
                    discash = 30;
                }
                porg[i] = discash;
                Debug.DrawRay(Sensors[i].transform.position, Sensors[i].transform.forward*hit.distance, Color.red);
            }
        }
        for (int i = 0; i < 9; i++) // 시뮬레이터 인터페이스
        {
            Transform pm = PSensorView.transform.GetChild(i);
            RawImage pimg = PSensorView.transform.Find($"p{i}").GetComponent<RawImage>();
            Text ptext = pm.transform.Find($"p{i}Text").GetComponent<Text>();
            if (porg[i] > 200)
            {
                pimg.color = danC;
            }
            else if(porg[i] > 130)
            {
                pimg.color = warC;
            }
            else
            {
                pimg.color = basC;
            }
            ptext.text = $"{porg[i]}";
        }
    }

    public static float p(int snum)
    {
        return porg[snum];
    }
}