using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Displacement : MonoBehaviour
{
    [SerializeField] Material mat;

    #region easing parameters
    [SerializeField] int duration;
    [SerializeField] float amp;
    int curTime = 0;
    float value = 0;
    bool isDoing = false;
    bool isUp = true;
    #endregion
    void Start()
    {
        mat.SetFloat("_width", Screen.width);
        mat.SetFloat("_height", Screen.height);
    }

    void Update()
    {
        if(isDoing)
        {
            value = Easing.easeInOutQuad((float)curTime / (float)duration) * amp;
            mat.SetFloat("_power", value * amp);

            if(isUp)
            {
                curTime++;
                if(curTime > duration) isUp = false;
            } else if(!isUp)
            {
                curTime = curTime - 10;
                if(curTime < 0) isDoing = false;
            }

        } else
        {
            curTime = 0;
            value = 0;
            mat.SetFloat("_power", value * amp);
            if(Input.GetKeyDown(KeyCode.D))
            {
                isUp = true;
                isDoing = true;
                amp = 1.0f;
            }
        }
    }
}
