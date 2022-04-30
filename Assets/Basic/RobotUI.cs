using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class RobotUI : MonoBehaviour
{
    public Text Target;
    int CaseNum = 0;
    bool StartRunning = false;

    public RobotMove MoveClass;

    /* ·Îº¿ ¹«ºù ¼±¾ð */
    //public void TD(float xpos, float ypos, int deg, float f_spd, float fw_spd, int stop2) => RobotMove.TD(xpos, ypos, deg, f_spd, fw_spd, stop2);
    /* ·Îº¿ ¹«ºù ¼±¾ð */


    private const string URL = "http://127.0.0.1:8800";

    public IEnumerator ServerGet()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(URL))
        {
            yield return request.SendWebRequest();
            Debug.Log($"{request.downloadHandler.text}");
        }
    }
    public void CaseUpOnClick()
    {
        CaseNum += 1;
        Target.text = $"{CaseNum}";
    }
    public void CaseDownOnClick()
    {
        CaseNum -= 1;
        Target.text = $"{CaseNum}";
    }

    public void StartOnClick()
    {
        StartRunning = true;
        StartCoroutine(MoveClass.caserunner(CaseNum));
    }
    public void ResetOnClick()
    {
        CaseNum = 0;
        Target.text = $"{CaseNum}";
    }

    public void AVRISPOnClick()
    {
        StartCoroutine(ServerGet());
        //TD(0, 0, 0, 0, 0, 0);
    }
}
