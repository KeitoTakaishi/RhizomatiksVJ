using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioReactivePosition : MonoBehaviour
{

    #region private data
    [Tooltip ("grown-offset")]
    [SerializeField] Vector3 pos; //変化する大きさ
    Vector3 initialPos;
    #endregion

    #region Easing Parameter
    [SerializeField] int duration;
    int curTime = 0;
    float v;
    bool isDoing = false;
    bool isUp = true;
    #endregion

    #region accessor
    public Vector3 Scale
    {
        set { pos = value; }
        get { return pos; }
    }
    #endregion

    void Start()
    {
        initialPos = this.transform.position;
    }

    void Update()
    {
        if(isDoing)
        {
            v = Easing.easeInOutQuad((float)curTime / (float)duration);
            this.transform.position = initialPos +  v * pos;

            if(isUp)
            {
                curTime++;
                if(curTime > duration) isUp = false;
            } else if(!isUp)
            {
                curTime = curTime - 2;
                if(curTime < 0) isDoing = false;
            }

        } else
        {
            curTime = 0;
            v = 0;
            this.transform.position = initialPos;
            if(Input.GetKeyDown(KeyCode.W) || OscData.kick == 1.0)
            {
                isUp = true;
                isDoing = true;
            }
        }
    }
}
