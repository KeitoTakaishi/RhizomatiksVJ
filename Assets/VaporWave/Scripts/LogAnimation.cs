using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogAnimation : MonoBehaviour
{

    #region privateData
    [SerializeField] RectTransform[] rt;
    [SerializeField] float scale;
    [SerializeField] float speed;
    [SerializeField] int interval;//サインの波形の周期
    [SerializeField] int yChangeInterval;//高さが変わるインターバル
    [SerializeField] Vector2 initOffset;
    [SerializeField] bool randomHeight = true;
    float offSet;
    float time;
    #endregion

    #region Accessor
    public bool RandomHeight
    {
        set { this.randomHeight = value; }
        get { return this.randomHeight; }
    }

    #endregion

    void Start()
    {
        offSet = 360.0f / (float)rt.Length;
        for(int i = 0; i < rt.Length; i++)
        {
            rt[i].anchoredPosition = new Vector2((float)i * initOffset.x, (float)i * initOffset.y);
        }
    }

    float offSesX = 1.0f;
    float offSesY = 0.0f;

    void Update()
    {
        for(int i = 0; i < rt.Length; i++)
        {
            var theta = time * speed + i * offSet;
            theta = theta * Mathf.Deg2Rad;


            //画面外に出た処理
            if(rt[i].anchoredPosition.x > Screen.width / 2.0)
            {
                offSesY = 0.0f;
                rt[i].anchoredPosition = new Vector2(-0.5f * Screen.width, scale * Mathf.Sin(theta));
                
            }

            rt[i].anchoredPosition = new Vector2(rt[i].anchoredPosition.x + offSesX, scale * Mathf.Sin(theta) + offSesY);
            if(Time.frameCount % interval == 0)
            {
                time++;
            }
        }

        if(Time.frameCount % yChangeInterval == 0)
        {
            if(RandomHeight)
            {
                offSesY = Random.Range(-1.0f * Screen.height / 9.0f, Screen.height / 8.0f);
            }
        }

    }
}
