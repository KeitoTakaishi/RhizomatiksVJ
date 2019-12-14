using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogAnimation : MonoBehaviour
{
   [SerializeField] RectTransform[] rt;
    [SerializeField] float scale;
    [SerializeField] float speed;
    [SerializeField] int interva;
    [SerializeField] Vector2 initOffset;
    float offSet;
    float time;
    
    void Start()
    {
        offSet = 360.0f / (float)rt.Length;
        for(int i = 0; i < rt.Length; i++)
        {
            rt[i].anchoredPosition = new Vector2((float)i * initOffset.x, (float)i * initOffset.y);
        }
    }

    float speedX = 1.0f;
    void Update()
    {
        for(int i = 0; i < rt.Length; i++)
        {
            var theta = time * speed + i * offSet;
            theta = theta * Mathf.Deg2Rad;

            if(rt[i].anchoredPosition.x > Screen.width / 2.0)
            {
                rt[i].anchoredPosition = new Vector2(-0.5f*Screen.width, scale * Mathf.Sin(theta));
            }
            rt[i].anchoredPosition = new Vector2(rt[i].anchoredPosition.x + speedX, scale * Mathf.Sin(theta));
            
            
            rt[i].anchoredPosition = new Vector2(rt[i].anchoredPosition.x + speedX, scale * Mathf.Sin(theta));
            if(Time.frameCount % interva == 0)
            {
                time++;
            }
        }
    }
}
