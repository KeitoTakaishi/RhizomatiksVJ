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

    void initPosition()
    {
        offSet = 360.0f / (float)rt.Length;

        float randomInit = Random.Range(-1.0f*Screen.width / 2.0f, Screen.width / 2.0f);
        for(int i = 0; i < rt.Length; i++)
        {
            rt[i].anchoredPosition = new Vector2((float)i * initOffset.x + randomInit, (float)i * initOffset.y);
        }
    }

    void initParames()
    {
        yChangeInterval = (int)Random.Range(15, 30);
    }
    void Start()
    {
        initPosition();
        initParames();
    }

    private void OnEnable()
    {
        Destroy(gameObject, 10.0f);
    }

    float offSesX = 1.0f;
    float baseheight = 0;
    void Update()
    {
        for(int i = 0; i < rt.Length; i++)
        {
            var theta = (time * speed + i * offSet) * Mathf.Deg2Rad;

            if(rt[i].anchoredPosition.x > Screen.width / 2.0)
            {
                outScreenWidth(i, theta, baseheight);
            }

            rt[i].anchoredPosition = new Vector2(rt[i].anchoredPosition.x + offSesX, scale * Mathf.Sin(theta) + baseheight);
            if(Time.frameCount % interval == 0)
            {
                time++;
            }
        }

        //高さの変更
        if(Time.frameCount % yChangeInterval == 0)
        {
            baseheight = Random.Range(-1.0f * Screen.height / 3.0f, Screen.height / 3.0f);
        }
    }

    void outScreenWidth(int i, float theta, float baseHeight)
    {
        rt[i].anchoredPosition = new Vector2(-0.5f * Screen.width, scale * Mathf.Sin(theta) + baseHeight);
    }
}
