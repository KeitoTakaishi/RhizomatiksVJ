using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReactiveRotate : MonoBehaviour
{
    #region private data
    [Tooltip("grown-offset")]
    [SerializeField] Vector3 rotate; //変化する大きさ
    Vector3 initialRotate;
    #endregion

    #region Easing Parameter
    [SerializeField] int duration;
    int curTime = 0;
    float v;
    bool isDoing = false;
    bool isUp = true;
    #endregion


    void Start()
    {
        initialRotate = this.transform.eulerAngles;
    }

    void Update()
    {
        Reaction();
    }

    void Reaction()
    {
        if(isDoing)
        {
            v = Easing.easeInOutQuad((float)curTime / (float)duration);
            this.transform.localEulerAngles = initialRotate + v * rotate;

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
            this.transform.localEulerAngles = initialRotate;
            if(Input.GetKeyDown(KeyCode.W) || OscData.kick == 1.0)
            {
                isUp = true;
                isDoing = true;
            }
        }
    }
}
