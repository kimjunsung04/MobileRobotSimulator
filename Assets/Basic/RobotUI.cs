using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotUI : MonoBehaviour
{
    public Text Target;
    int CaseNum = 0;

    public RobotMove MoveClass;

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
        StartCoroutine(MoveClass.caserunner(CaseNum));
    }
    public void ResetOnClick()
    {
        CaseNum = 0;
        Target.text = $"{CaseNum}";
    }
}
