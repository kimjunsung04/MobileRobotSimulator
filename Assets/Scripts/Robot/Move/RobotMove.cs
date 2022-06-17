using System;
using System.Collections;
using UnityEngine;
public class RobotMove : MonoBehaviour
{
    public float[] distance = new float[3] { 0, 0, 0 };
    private static float[] porg = new float[9];
    public MainSensor MainSensor;

    public float f_agl = 0; // 기준각
    bool step_end = false; // 스텝 끝났는지 체크
    float[] pspd = new float[3] { 0, 0, 0 };
    float[] spd = new float[3] { 0, 0, 0 };


    private static AudioSource audioSource; // 비프

    private IEnumerator Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        yield return null;
    }

    public IEnumerator StepHandler(string movef, params float[] disl)
    {
        if (movef == "H")
        {
            yield return StartCoroutine(H(disl[0], disl[1], disl[2]));
        }
        else if (movef == "wp")
        {
            yield return StartCoroutine(wp(disl[0], disl[1]));
        }
        else if (movef == "pb")
        {
            yield return StartCoroutine(pb(disl[0], disl[1], disl[2], disl[3]));
            clear();
        }
        else if (movef == "TD")
        {
            yield return StartCoroutine(TD(disl[0], disl[1], disl[2], disl[3], disl[4], disl[5]));
            clear();
        }
        else if (movef == "CC")
        {
            yield return StartCoroutine(CC(disl[0], disl[1], disl[2], disl[3]));
            clear();
        }
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

    public IEnumerator TD(float xpos, float ypos, float deg, float f_spd, float fw_spd, float stop2)
    {
        float xspd = f_spd;
        float yspd = f_spd;
        if (xpos != 0) // x값이 마이너스 일때
        {
            if (xpos <= 0) xspd = -f_spd;
        }

        if (ypos != 0) // x값이 마이너스 일때
        {
            if (ypos <= 0) yspd = -f_spd;
        }

        if (deg != 0 && fw_spd == 0)
        {
            fw_spd = (float)(deg / (hypot(xpos, ypos) / Math.Abs(f_spd)));
        }
        if (xpos == 0) xspd = 0;
        if (ypos == 0) yspd = 0;
        while (true)
        {
            if(ff(xpos, ypos, deg))
            {
                break;
            }
            if (deg != 0) // 기준각을 distance로 변환
            {
                if (deg > 0)
                    f_agl = distance[2] - (Math.Abs(distance[2]) * 2);
                else if (deg < 0)
                    f_agl = distance[2];
            }
            if (distance[0] >= Math.Abs(xpos)) xspd = 0;
            if (distance[1] >= Math.Abs(ypos)) yspd = 0;
            if (distance[2] >= Math.Abs(deg)) fw_spd = 0;
            StartCoroutine(H(xspd, yspd, fw_spd));
            yield return 0;
        }
        yield return new WaitForSeconds(0.05f);
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
        distance[2] += Time.deltaTime * Math.Abs(vw);

        yield return 0;
    }

    public IEnumerator pb(float num, float mm, float speed, float mode)
    {
        float err = 0, dir = 1;
        if (mm < 0)
        {
            dir = -1;
            mm = -mm;
        }
        err = (mm - p((int)num)) * dir;
        if (mode != 0 && (p((int)num) < mm - 30 || p((int)num) > mm + 60)) err = 0;

        StartCoroutine(H(speed, err, err * 0.5f));
        yield return 0;
    }

    public IEnumerator wp(float mode, float speed)
    {
        float[] sen = new float[] { 130, 130 };
        float err = 0, err2 = 0;

        if (mode == 40)
        {
            sen[0] = 180;
            sen[1] = 180;
        }

        if (p(1) > sen[0]) err = (p(1) - sen[0]);
        else err = 0;
        if (p(8) > sen[1]) err2 = (sen[1] - p(8));
        else err2 = 0;

        StartCoroutine(H(speed, 5f * (err + err2), 0));
        yield return 0;
    }

    public IEnumerator CC(float dir, float dir2, float mm, float er)
    {
        float val = 0;
        float[] s = new float[2] { 0, 0 };
        float[] err = new float[3] { 0, 0, 0 };
        float total;

        clear();
        while (true)
        {
            if (dir == 1)
            {
                err[1] = p(3) - 230;
                if (mm!=0)
                {
                    err[0] = p((int)dir2) - mm;
                    if (dir2 == 0) err[0] *= -1;
                }
                val = 44.5f;
                s[0] = 1;
                s[1] = 3;
            }
            else if (dir == 8)
            {
                err[1] = 230 - p(6);
                if (mm!=0)
                {
                    err[0] = p((int)dir2) - mm;
                    if (dir2 == 0) err[0] *= -1;
                }
                val = 44.5f;//30
                s[0] = 8;
                s[1] = 6;
            }
            else if (dir == 5)
            {
                err[0] = p(4) - 235;
                if (mm!=0)
                {
                    err[1] = p((int)dir2) - mm;
                    if (dir2 >= 6) err[1] *= -1;
                }
                val = 49.5f; //51~54     R(val up) / L(val down)
                s[0] = 4;
                s[1] = 5;
            }

            total = (float)p((int)s[0]) / ((float)(p((int)s[0]) + p((int)s[1])) / 100);
            err[2] = Math.Abs(val - total);

            if (total < val && dir != 8) err[2] *= -1;
            else if (total > val && dir == 8) err[2] *= -1;

            if (Math.Abs(err[0]) <= er && Math.Abs(err[1]) <= er && Math.Abs(err[2]) <= er) break;
            StartCoroutine(H(err[0] * 3, err[1] * 2, err[2] * 2));
            yield return 0;
        }
    }

    public void beep(int num, int time)
    {
        num = Math.Abs(num);
        for (int i = 0; i < num; i++)
        {
            audioSource.Play();
        }
    }
    
    public bool ff(float mm0, float mm1, float mm2)
    {
        float[] total_distance = new float[3];
        total_distance[0] = mm0;
        total_distance[1] = mm1;
        total_distance[2] = mm2;

        if((distance[0] >= Math.Abs(total_distance[0])) &&
            (distance[1] >= Math.Abs(total_distance[1])) &&
            (distance[2] >= Math.Abs(total_distance[2])))
        {
            return true;
        }
        return false;
    }

    public void clear()
    {
        f_agl = 0;
        for (int i = 0; i < 3; i++) distance[i] = 0;
    }

    /*센서영역*/
    public static float p(int snum)
    {
        return porg[snum];
    }
    /*센서영역Ed*/

    void Update()
    {
        porg = MainSensor.porg;
    }

    /* 필요 연산함수 구현 */
    double hypot(double x, double y)
    {
        return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
    }
}