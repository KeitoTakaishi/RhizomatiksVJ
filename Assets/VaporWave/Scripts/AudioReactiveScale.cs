using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VaporWave
{
    [RequireComponent(typeof(Easing))]
    public class AudioReactiveScale : MonoBehaviour
    {

        #region private data
        //float value;
        [SerializeField] Vector3 scale; //変化する大きさ
        [SerializeField] GameObject osc;
        OSCManager oscManager;
        Easing easing;
        Vector3 initialScale;
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
            oscManager = osc.GetComponent<OSCManager>();
            initialScale = this.transform.localScale;
            easing = this.GetComponent<Easing>();
        }

        void Update()
        {
            if(isDoing)
            {
                v = easing.easeInOutQuad((float)curTime / (float)duration);
                this.transform.localScale = initialScale +  v * scale;

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
                this.transform.localScale = initialScale;
                if(Input.GetKeyDown(KeyCode.W) || oscManager.oscData.Kick == 1.0)
                {
                    isUp = true;
                    isDoing = true;
                }
            }
        }
    }
}