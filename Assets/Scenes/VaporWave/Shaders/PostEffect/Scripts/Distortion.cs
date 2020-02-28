using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distortion : MonoBehaviour
{
    [SerializeField] Material mat;

    #region easing parameters
    [SerializeField] int duration;
    Easing easing;
    int curTime = 0;
    float value = 0;
    bool isDoing = false;
    bool isUp = true;
    float amp = 0.0f;
    #endregion
    void Start()
    {
        easing = this.GetComponent<Easing>();

        mat.SetFloat("_width", Screen.width);
        mat.SetFloat("_height", Screen.height);
    }

    void Update()
    {
        if(isDoing)
        {
            value = Easing.easeInOutQuad((float)curTime / (float)duration) * amp;
            mat.SetFloat("_power", value);
            
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
            value = 0;
            mat.SetFloat("_power", value);
            if(Input.GetKeyDown(KeyCode.T))
            {
                isUp = true;
                isDoing = true;
                amp = 1.0f;
            }
        }
    }
}
