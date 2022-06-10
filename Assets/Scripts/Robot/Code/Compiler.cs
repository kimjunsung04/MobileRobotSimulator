using UnityEngine;
using RoslynCSharp;

public class Compiler : MonoBehaviour
{
    private static ScriptDomain domain;
    private static ScriptProxy proxy = null;
    public static bool comck = false;

    public static void RunCode(string source, string csenum)
    {
        if (comck) // 컴파일 여부 체크
        {
            CodeCompile(source);
        }
        proxy.Call("caserunner", $"{csenum}");
    }

    public static void CodeCompile(string source)
    {
        GameObject gameObject = new GameObject();

        domain = ScriptDomain.CreateDomain("RobotRunDomain", true);

        ScriptType type = domain.CompileAndLoadMainSource(source, ScriptSecurityMode.UseSettings);
        proxy = type.CreateInstance(gameObject);
        comck = false;
    }
}
