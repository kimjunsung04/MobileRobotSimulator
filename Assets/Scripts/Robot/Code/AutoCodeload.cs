using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AutoCodeload : MonoBehaviour
{
    // Start is called before the first frame update

    public BaseUI BaseUI;
    public RobotMove RobotMove;
    public Compiler Compiler;

    private const string URL = "http://127.0.0.1:8800/download";

    private long requestCode;
    public string SourceCodeStr;

    public IEnumerator ServerGet()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(URL))
        {
            yield return request.SendWebRequest();
            requestCode = request.responseCode;
            if (requestCode == 200)
            {
                SourceCodeStr = request.downloadHandler.text;
                Compiler.comck = true;
            }
        }
    }

    public IEnumerator CodeGet()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(URL))
        {
            yield return request.SendWebRequest();
            requestCode = request.responseCode;
            if (requestCode == 200 && (SourceCodeStr!=request.downloadHandler.text))
            {
                BaseUI.PopupShow("알림", "새로운코드 감지됨!", 3);
                SourceCodeStr = request.downloadHandler.text;
                Compiler.comck = true;
            }
        }
        requestCode = 0;
        yield return new WaitForSeconds(1);
        yield return CodeGet();
    }

    IEnumerator Start()
    {
        yield return ServerGet();
        if (requestCode == 200)
        {
            BaseUI.PopupShow("알림", "서버통신 성공", 3);
        }
        else
        {
            BaseUI.PopupShow("알림", "서버통신 실패", 3);
        }
        requestCode = 0;
        yield return CodeGet();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
