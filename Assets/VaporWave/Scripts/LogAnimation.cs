using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogAnimation : MonoBehaviour
{
   [SerializeField] RectTransform[] rt;
    [SerializeField] float scale;
    [SerializeField] float speed;
    [SerializeField] int interva;
    float offSet;
    float time;
    
    void Start()
    {
        offSet = 360.0f / (float)rt.Length;
    }

    void Update()
    {
        for(int i = 0; i < rt.Length; i++)
        {
            var theta = time * speed + i * offSet;
            theta = theta * Mathf.Deg2Rad;
            rt[i].anchoredPosition = new Vector2(rt[i].anchoredPosition.x, scale * Mathf.Sin(theta));
            if(Time.frameCount % interva == 0) time++;

        }
    }
}
