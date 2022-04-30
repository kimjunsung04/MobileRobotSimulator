using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Newtonsoft.Json.Linq;

public class RobotMove : MonoBehaviour
{
    float[] org_pos = new float[3] { 0, 0, 0 };
    float[] distance = new float[3] { 0, 0, 0 };
    float start_deg;

    public string SourceCodeStr;

    float distanceforward;
    float distanceright;

    bool reck = false;
    bool step_end = false;
    Vector3 target; // TD Ÿ�� ����

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

    public IEnumerator caserunner(int csenum) // ���̽� �ڷ�ƾ �����Լ�
    {
        //JObject SourceCode = JObject.Parse(SourceCodeStr);
        //Debug.Log(SourceCode["source"]);
        yield return StartCoroutine(caserun(csenum));
    }

    public IEnumerator caserun(int casenum) // ���� ���̽�
    {
        switch (casenum)
        {
            case 0:
                while (true)
                {
                    yield return StartCoroutine(StepHandler("H", 300, 0, 0));
                    if (distanceforward > 500)
                    {
                        break;
                    }
                }
                break;
            case 1:
                while (true)
                {
                    yield return StartCoroutine(StepHandler("H", 300, -300, 0));
                    if (distanceforward > 500)
                    {
                        break;
                    }
                }
                break;
        }
    }

    private void startset() // �ʹ� ��ǥ����, �������� �ʱ�ȭ
    {
        step_end = false;
        reck = false;
        org_pos[0] = transform.position.x;
        org_pos[1] = transform.position.z;
        org_pos[2] = transform.localEulerAngles.y;
    }

    private IEnumerator H(float vx, float vy, float vw)
    {
        vx = vx / 1000;
        vy = vy / 1000;
        vw = vw / 1000;

        transform.Translate((transform.forward * 1) * Time.deltaTime * vx, Space.World);
        transform.Translate((transform.right * 1) * Time.deltaTime * vy, Space.World);
        transform.Rotate((transform.up * 1000) * Time.deltaTime * vw, Space.World);

        /* distance */
        distanceforward += ((transform.forward * 1) * Time.deltaTime * vx).magnitude * 1000;
        distanceright += ((transform.right * 1) * Time.deltaTime * vy).magnitude * 1000;

        yield return 0;
    }

    private IEnumerator TD(float xpos, float ypos, float deg, float fw_spd)
    {
        /* xy�� ������ TD */
        /*
        
        start_deg = transform.localEulerAngles.y;
        IEnumerator start = TD(800, 200,0, 0.5f);
        yield return StartCoroutine(start);
        IEnumerator start1 = TD(200, 300, 0, 0.5f);
        yield return StartCoroutine(start1);

         */
        startset();
        xpos = xpos / 1000; // xpos ����Ƽ ��ǥ�� ����
        ypos = ypos / 1000; // ypos ����Ƽ ��ǥ�� ����
        fw_spd = fw_spd / 1000;
        if (start_deg == 0)
        {
            target = new Vector3(transform.position.x + ypos, transform.position.y, transform.position.z + xpos);
        }
        else if (start_deg == 90)
        {
            ypos -= (ypos * 2); // ypos �¿캯��ó��(+-����)
            target = new Vector3(transform.position.x + xpos, transform.position.y, transform.position.z + ypos);
        }
        else if (start_deg == 180)
        {
            ypos -= (ypos * 2); // ypos �¿캯��ó��(+-����)
            xpos -= (xpos * 2); // xpos ����ó��(+-����)
            target = new Vector3(transform.position.x + ypos, transform.position.y, transform.position.z + xpos);
        }
        else if (start_deg == 270)
        {
            xpos -= (xpos * 2); // xpos ����ó��(+-����)
            target = new Vector3(transform.position.x + xpos, transform.position.y, transform.position.z + ypos);
        }
        float t = 0;
        while (t <= 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, fw_spd * Time.deltaTime); // x y ó��
            t += Time.deltaTime;
            yield return 0; // wait one frame to refresh display
        }

        yield return new WaitForSeconds(1); // wait one second
        step_end = true;
        // call again TweenPosition as a coroutine
    }

    void Update()
    {
        // ISP ����
        if (step_end && !reck)
        {
            distanceforward = 0;
            distanceright = 0;
            reck = true;
        }
        distance[0] = org_pos[0] - transform.position.x;
        Debug.DrawRay(transform.position, transform.forward, Color.red, 100); // ���� üũ(����׿�)
    }
}