using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ESPAPI : MonoBehaviour
{
    public static ESPAPI instance;

    public void Awake()
    {
        if (instance == null) instance = this;
    }

    public IEnumerator SendMessage(int mode)
    {
        string mode_str = "stop";
        if (mode == 0)
        {
            mode_str = "stop";
        }
        else if (mode == 1)
        {
            mode_str = "true";
        }
        else if (mode == -1)
        {
            mode_str = "false";
        }

        Debug.Log(mode_str);
        using (var req = UnityWebRequest.Get("http://192.168.30.3/?" + mode_str))
//         using (var req = UnityWebRequest.Get("http://localhost:3000/" + mode_str + "/   "))
        {
            yield return req.SendWebRequest();
        }
    }

    public void SendMessageLater(float second, int mode)
    {
        StartCoroutine(DelayCoroutine(second, () =>
        {
            StartCoroutine(instance.SendMessage(mode));
        }
            )
        );
    }
    private IEnumerator DelayCoroutine(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }
}