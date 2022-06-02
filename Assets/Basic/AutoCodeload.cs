using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AutoCodeload : MonoBehaviour
{
    // Start is called before the first frame update

    public BaseUI BaseUI;
    public RobotMove RobotMove;


    private const string URL = "http://127.0.0.1:8800/download";

    private long requestCode;

    public IEnumerator ServerGet()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(URL))
        {
            yield return request.SendWebRequest();
            requestCode = request.responseCode;
            if (requestCode == 200)
            {
                RobotMove.SourceCodeStr = request.downloadHandler.text;
            }
        }
    }
    IEnumerator Start()
    {
        yield return ServerGet();
        if (requestCode == 200)
        {
            BaseUI.PopupShow("�˸�", "������� ����", 3);
        }
        else
        {
            BaseUI.PopupShow("�˸�", "������� ����", 3);
        }
        requestCode = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
}