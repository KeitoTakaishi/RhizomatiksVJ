using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uOSC;


[System.Serializable]
public struct OscData
{
    [SerializeField]
    float low;
    [SerializeField]
    float kick;
    [SerializeField]
    float rythm;
    [SerializeField]
    float snare;

    public OscData(float l, float k, float r, float s)
    {
        low = l;
        kick = k;
        rythm = r;
        snare = s;
    }

    public float Low
    {
        set { this.low = value; }
        get { return this.low; }
    }

    public float Kick
    {
        set { this.kick = value; }
        get { return this.kick; }
    }


    public float Rythm
    {
        set { this.rythm = value; }
        get { return this.rythm; }
    }

    public float Snare
    {
        set { this.snare = value; }
        get { return this.snare; }
    }
};
public class OSCManager : MonoBehaviour
{

    [SerializeField]
    public OscData oscData; 
    void Start()
    {
        oscData = new OscData(0, 0, 0, 0);
        var server = GetComponent<uOscServer>();
        server.onDataReceived.AddListener(OnDataReceived);
    }

    void Update() { 
        
       
    }

    void OnDataReceived(Message message)
    {

        // address
        var address = message.address;
        switch (address)
        {
            case "/Low":
                //Debug.Log(message.values[0]);
                oscData.Low = float.Parse (message.values[0].ToString());
                break;

            case "/Kickdetection":
                //Debug.Log(message.values[0]);
                oscData.Kick = float.Parse(message.values[0].ToString());
                break;

            case "/Rythm":
                //Debug.Log(message.values[0]);
                oscData.Rythm = float.Parse(message.values[0].ToString());
                break;

            case "/Snaredetection":
                //Debug.Log(message.values[0]);
                oscData.Snare= float.Parse(message.values[0].ToString());
                break;
        } 
    }
}
