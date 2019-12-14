using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uOSC;


[System.Serializable]
static public class OscData
{
    public static float low;
    public static float kick;
    public static float rythm;
    public static float snare;

    /*
    public OscData(float l, float k, float r, float s)
    {
        low = l;
        kick = k;
        rythm = r;
        snare = s;
    }
    */
    /*
    public float Low
    {
        set { low = value; }
        get { return this.low; }
    }

    public float Kick
    {
        set { kick = value; }
        get { return this.kick; }
    }


    public float Rythm
    {
        set { rythm = value; }
        get { return rythm; }
    }

    public float Snare
    {
        set { snare = value; }
        get { return snare; }
    }
    */
};
public class OSCManager : MonoBehaviour
{

    void Start()
    {
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
                OscData.low = float.Parse (message.values[0].ToString());
                break;

            case "/Kickdetection":
                //Debug.Log(message.values[0]);
                OscData.kick = float.Parse(message.values[0].ToString());
                break;

            case "/Rythm":
                //Debug.Log(message.values[0]);
                OscData.rythm = float.Parse(message.values[0].ToString());
                break;

            case "/Snaredetection":
                //Debug.Log(message.values[0]);
                OscData.snare= float.Parse(message.values[0].ToString());
                break;
        } 
    }
}
