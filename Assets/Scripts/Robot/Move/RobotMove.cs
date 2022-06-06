using System.Collections;
using UnityEngine;
using System.Threading;
using Newtonsoft.Json.Linq;

public class RobotMove : MonoBehaviour
{
    float[] org_pos = new float[3] { 0, 0, 0 };
    public float[] distance = new float[3] { 0, 0, 0 };

    bool reck = false; // 리셋체크
    bool step_end = false; // 스텝 끝났는지 체크
    private JObject SourceCode;

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

    public IEnumerator StepHandler(string movef, params int[] disl)
    {
        startset();
        if (movef == "H")
        {
            yield return StartCoroutine(H(disl[0], disl[1], disl[2]));
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
        vw = vw / 1000;

        transform.Translate((transform.forward * 1) * Time.deltaTime * vx, Space.World);
        transform.Translate((transform.right * 1) * Time.deltaTime * vy, Space.World);

        /* distance */
        distance[0] += ((transform.forward * 1) * Time.deltaTime * vx).magnitude * 1000;
        distance[1] += ((transform.right * 1) * Time.deltaTime * vy).magnitude * 1000;

        yield return 0;
    }

    void Update()
    {
        // ISP 역할
        if (step_end && !reck)
        {
            for (int i = 0; i < 3; i++) distance[i] = 0;
            reck = true;
        }
    }
}