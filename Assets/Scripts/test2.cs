using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test2 : MonoBehaviour
{
    public static RobotMove robot;
    public static float[] distance;
    public static int CaseNum;
    // Start is called before the first frame update

    public static int AddYourselfTo(int casenum)
    {
        CaseNum = casenum;
        return CaseNum;
    }

    void Start()
    {
        AddYourselfTo(0);
        robot = GameObject.Find("RobotModel").GetComponent<RobotMove>();
        StartCoroutine(caserunner(CaseNum));
    }

    // Update is called once per frame
    void Update()
    {
        distance = robot.distance;
    }

    public static IEnumerator StepHandler(string movef, params int[] disl)
    {
        yield return robot.StartCoroutine(robot.StepHandler(movef, disl));
    }

    public static IEnumerator caserunner(int csenum) // 케이스 코루틴 실행함수
    {
        yield return robot.StartCoroutine(caserun(csenum));
    }

    public static IEnumerator caserun(int casenum) // 메인 케이스
    {
        switch (casenum)
        {
            case 0:
                while (true)
                {
                    yield return robot.StartCoroutine(StepHandler("H", 300, 0, 0));
                    if (distance[0] > 800)
                    {
                        break;
                    }
                }
                break;
            case 1:
                while (true)
                {
                    yield return robot.StartCoroutine(StepHandler("H", 300, 0, 0));
                    if (distance[0] > 500)
                    {
                        break;
                    }
                }
                break;
        }
    }
}