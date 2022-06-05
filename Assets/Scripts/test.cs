using UnityEngine;
using System;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text;

public class test : MonoBehaviour
{
    private void Start()
    {
        RobotMove robot;
        robot = GameObject.Find("RobotModel").GetComponent<RobotMove>();



        var assembly = Compile(@"
            using System.Collections;
            using System.Collections.Generic;
            using UnityEngine;

            public class RuntimeCompiled : MonoBehaviour
            {
                public static RobotMove robot;
                public static float[] distance;
                // Start is called before the first frame update

                public static RuntimeCompiled AddYourselfTo(GameObject host)
                {
                    return host.AddComponent<RuntimeCompiled>();
                }

                void Start()
                {
                    robot = GameObject.Find(""RobotModel"").GetComponent<RobotMove>();
                    StartCoroutine(caserun(0));
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

                public IEnumerator caserun(int casenum) // ���� ���̽�
                {
                    switch (casenum)
                    {
                        case 0:
                            while (true)
                            {
                                yield return StartCoroutine(StepHandler(""H"", 300, 0, 0));
                                if (distance[0] > 800)
                                {
                                    break;
                                }
                            }
                            break;
                        case 1:
                            while (true)
                            {
                                yield return StartCoroutine(StepHandler(""H"", 300, 0, 0));
                                if (distance[0] > 500)
                                {
                                    break;
                                }
                            }
                            break;
                    }
                }
            }");

        var runtimeType = assembly.GetType("RuntimeCompiled");
        var method = runtimeType.GetMethod("AddYourselfTo");
        var del = (Func<GameObject, MonoBehaviour>)
            Delegate.CreateDelegate(
            typeof(Func<GameObject, MonoBehaviour>),
            method
            );

        // We ask the compiled method to add its component to this.gameObject
        var addedComponent = del.Invoke(gameObject);

        // The delegate pre-bakes the reflection, so repeated calls don't
        // cost us every time, as long as we keep re-using the delegate.
    }

    public static Assembly Compile(string source)
    {
        // Replace this Compiler.CSharpCodeProvider wth aeroson's version
        // if you're targeting non-Windows platforms:
        var provider = new CSharpCodeProvider();
        var param = new CompilerParameters();

        // Add ALL of the assembly references
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            string cash_d = assembly.Location.ToString();
            if (!cash_d.Contains("mscorlib.dll"))
            {
                param.ReferencedAssemblies.Add(assembly.Location);
            }
        }

        // Or, uncomment just the assemblies you need...

        // System namespace for common types like collections.
        //param.ReferencedAssemblies.Add("System.dll");

        // This contains methods from the Unity namespaces:
        //param.ReferencedAssemblies.Add("UnityEngines.dll");

        // This assembly contains runtime C# code from your Assets folders:
        // (If you're using editor scripts, they may be in another assembly)
        //param.ReferencedAssemblies.Add("CSharp.dll");


        // Generate a dll in memory
        param.GenerateExecutable = false;
        param.GenerateInMemory = true;

        // Compile the source
        var result = provider.CompileAssemblyFromSource(param, source);

        if (result.Errors.Count > 0)
        {
            var msg = new StringBuilder();
            foreach (CompilerError error in result.Errors)
            {
                msg.AppendFormat("Error ({0}): {1}\n",
                  error.ErrorNumber, error.ErrorText);
            }
            throw new Exception(msg.ToString());
        }

        // Return the assembly
        return result.CompiledAssembly;
    }
}