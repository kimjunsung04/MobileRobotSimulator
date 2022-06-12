using System.Collections;
using UnityEngine;
using System.Threading;
using System;
public class RobotMove : MonoBehaviour
{
    float[] org_pos = new float[3] { 0, 0, 0 };
    public float[] distance = new float[3] { 0, 0, 0 };
    private static float[] porg = new float[9];
    public MainSensor MainSensor;
    
    public float f_agl = 0; // 기준각
    bool reck = false; // 리셋체크
    bool step_end = false; // 스텝 끝났는지 체크
    float[] pspd = new float[2];

    //void Start()
    //{
    //    org_position[0] = transform.position.x;
    //    org_position[1] = transform.position.y;
    //    org_position[2] = transform.forward.y;
    //    target = transform.position;
    //    target.x = transform.position.x + 0.5f;
    //    target.z = transform.position.z + 0.5f;
    //
    //    targetw = transform.localEulerAngles;
    //    targetw.y = transform.localEulerAngles.y;
    //}

    private IEnumerator Start()
    {
        yield return null;
    }

    public IEnumerator StepHandler(string movef, params float[] disl)
    {
        startset();
        if (movef == "H")
        {
            yield return StartCoroutine(H(disl[0], disl[1], disl[2]));
        }
        else if (movef == "wp")
        {
            yield return StartCoroutine(wp(disl[0], disl[1]));
        }
        step_end = true;
    }

    public IEnumerator caserunner(int csenum) // 케이스 코루틴 실행함수
    {
        yield return StartCoroutine(caserun(csenum));
    }

    public IEnumerator caserun(int casenum) // 메인 케이스
    {
        switch (casenum)
        {
            case 0:
                while (true)
                {
                    yield return StartCoroutine(StepHandler("H", 300, 0, 0));
                    if (distance[0] > 300)
                    {
                        break;
                    }
                }
                break;
            case 1:
                while (true)
                {
                    yield return StartCoroutine(StepHandler("H", 300, -300, 0));
                    if (distance[0] > 500)
                    {
                        break;
                    }
                }
                break;
        }
    }

    private void startset() // 초반 좌표설정, 스탭진행 초기화
    {
        step_end = false;
        reck = false;
        org_pos[0] = transform.position.x;
        org_pos[1] = transform.position.z;
        org_pos[2] = transform.localEulerAngles.y;
    }

    public IEnumerator H(float vx, float vy, float vw)
    {
        vx = vx / 1000;
        vy = vy / 1000;
        float[] v = new float[2] { vx, vy };

        if (f_agl != 0)
        {
            v[0] = (float)(Math.Cos(f_agl * Math.PI / 180) * vx - Math.Cos((90 - f_agl) * Math.PI / 180) * vy);
            v[1] = (float)(Math.Sin(f_agl * Math.PI / 180) * vx + Math.Sin((90 - f_agl) * Math.PI / 180) * vy);
        }

        transform.Translate((transform.forward * 1) * Time.deltaTime * v[0], Space.World);
        transform.Translate((transform.right * 1) * Time.deltaTime * v[1], Space.World);
        transform.Rotate(new Vector3(0, Time.deltaTime * vw, 0));

        /* distance */
        distance[0] += ((transform.forward * 1) * Time.deltaTime * vx).magnitude * 1000;
        distance[1] += ((transform.right * 1) * Time.deltaTime * vy).magnitude * 1000;
        distance[2] += Time.deltaTime * vw;

        yield return 0;
    }

    public IEnumerator wp(float mode, float speed)
    {
        float[] sen = new float[] { 0.23f, 0.23f };
        float err = 0, err2 = 0;

        if (mode == 40)
        {
            sen[0] = 0.17f;
            sen[1] = 0.17f;
        }

        if (p(1) < sen[0]) err = (sen[0]- p(1)) *1000;
        else err = 0;
        if (p(8) < sen[1]) err2 = (p(8)-sen[1])*1000;
        else err2 = 0;

        StartCoroutine(H(speed, 5f * (err + err2), 0));
        yield return 0;
    }

    /*센서영역*/
    public static float p(int snum)
    {
        return porg[snum];
    }
    /*센서영역Ed*/

    void Update()
    {

        // ISP 역할
        if (step_end && !reck)
        {
            for (int i = 0; i < 3; i++) distance[i] = 0;
            reck = true;
        }
        porg = MainSensor.porg;
    }
}