using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotUI : MonoBehaviour
{
    public Text Target;
    public int CaseNum = 0;

    public RobotMove MoveClass;
    public Compiler Compiler;
    public AutoCodeload AClass;

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
        string source = AClass.SourceCodeStr;
        Compiler.RunCode(source, $"{CaseNum}");
    }
    public void ResetOnClick()
    {
        CaseNum = 0;
        Target.text = $"{CaseNum}";
    }
}
