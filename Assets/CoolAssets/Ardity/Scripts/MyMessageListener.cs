using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMessageListener : MonoBehaviour
{
    [SerializeField] GameObject mover;
    [SerializeField] float xMax;
    [SerializeField] float yMax;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMessageArrived(string msg)
    {
        Debug.Log("Arrived: " + msg);
        // Debug.Log(msg[1] + " " + msg[2]);
        // char[] = msg.ToCharArray();
        string xCoord, yCoord;
        // xCoord.Add(char[1]);
        xCoord = msg.Substring(1,2);
        yCoord = msg.Substring(3,2);
        Debug.Log("x: " + xCoord +", Y: " + yCoord);
        float xSpot = Remap(float.Parse(xCoord), 48f, 56f, -1f, 1f);
        float ySpot = Remap(float.Parse(yCoord), 48f, 56f, -1f, 1f);
        Instantiate(mover, new Vector3(xSpot * xMax, 5f, ySpot * yMax), Quaternion.identity);
    }

    void OnConnectionEvent(bool success)
    {
        Debug.Log(success? "Device connected" : "Device disconnected");
    }

    float Remap (float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
