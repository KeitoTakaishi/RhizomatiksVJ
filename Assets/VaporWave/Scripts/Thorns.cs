using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorns : MonoBehaviour
{

    [SerializeField] GameObject model;
    [SerializeField] Shader shader;
    Material surface;


    #region easing parameters
    [SerializeField] int duration;
    Easing easing;
    int curTime = 0;
    float value = 0;
    bool isDoing = false;
    bool isUp = true;
    #endregion



    private void Awake()
    {
        model.GetComponent<MeshFilter>().mesh.RecalculateNormals();
    }

    void Start()
    {
        surface = model.GetComponent<MeshRenderer>().material;
        easing = this.GetComponent<Easing>();
        
    }

    void Update()
    {
       
        if(isDoing)
        {
            value = easing.easeOutCubic( (float)curTime / (float)duration);
            surface.SetFloat("coef", value);
            if(isUp)
            {
                curTime++;
                if(curTime > duration) isUp = false;
            }
            else if(!isUp)
            {
                curTime--;
                if(curTime < 0) isDoing = false;
            } 
        }
        else
        {
            curTime = 0;
            value = 0;
            surface.SetFloat("coef", value);
            if(Input.GetKeyDown(KeyCode.T))
            {
                isUp = true;
                isDoing = true;
                var mode = Random.RandomRange(0, 4);
                surface.SetInt("_Mode", mode);
               
            }
        }
    }
}
