using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Move : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Stop
        SendMessage(0, 0);
        SendMessage(1, 0);

        //0,0
        Invoke("LeftDown", 2.0f);
        Invoke("RightDown", 2.0f);
        Invoke("LeftStop", 4.0f);
        Invoke("RightStop", 4.0f);
        //-2,-2
        Invoke("LeftUp", 7.5f);
        Invoke("RightUp", 7.5f);
        Invoke("LeftStop", 10.5f);
        Invoke("RightStop", 10.5f);
        //1,1
        Invoke("RightDown", 11.2f);
        Invoke("RightStop", 12.2f);
        //1,0
        Invoke("LeftDown", 12.5f);
        Invoke("RightUp", 12.5f);
        Invoke("LeftStop", 13.5f);
        Invoke("RightStop", 13.5f);
        // 0,1
        Invoke("RightDown", 13.7f);
        Invoke("RightStop", 14.0f);
        // 0, 0.7
        Invoke("RightUp", 14.2f);
        Invoke("RightStop", 14.5f);
        // 0, 1

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LeftStop() {SendMessage(0, 0);}
    public void LeftUp() {SendMessage(0, 1);}
    public void LeftDown() {SendMessage(0, 2);}
    public void RightStop() {SendMessage(1, 0);}
    public void RightUp() {SendMessage(1, 1);}
    public void RightDown() {SendMessage(1, 2);}

    // id: 0 = left, 1 = right
    // mode: 0 = stop, 1 = positive, 2 = negative
    IEnumerator SendMessage(int id, int mode) {
        using (var req = UnityWebRequest.Get("http://192.168.30.3/?" + id.ToString() + mode.ToString() + "?end"))
        {
            yield return req.SendWebRequest();
        }
    }
}
