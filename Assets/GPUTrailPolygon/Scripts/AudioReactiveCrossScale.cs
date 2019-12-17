using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReactiveCrossScale : MonoBehaviour
{
    [SerializeField] GameObject cross;
    [SerializeField] Vector2 gridNum;
    [SerializeField] Vector2 gridSize;
    List<GameObject> crosses;


    #region private data
    [Tooltip("grown-offset")]
    //[SerializeField] Vector3 scale; //変化する大きさ
    public float scale;
    Vector3 initialScale;
    #endregion

    #region Easing Parameter
    [SerializeField] int duration;
    int curTime = 0;
    float v;
    bool isDoing = false;
    bool isUp = true;
    #endregion

    [SerializeField] public bool isRotate = false;

    void Start()
    {
        crosses = new List<GameObject>();
        Vector2 delta = new Vector2(gridSize.x/gridNum.x, gridSize.y/ gridNum.y);    
        for(int i = 0; i < gridNum.x; i++)
        {
            for(int j = 0; j < gridNum.y; j++)
            {
                
                var c = Instantiate(cross, new Vector3(
                    (j*2.0f-gridNum.x+1)*delta.x,
                    (i*2.0f-gridNum.y+1)*delta.y,
                    0.0f),Quaternion.identity) as GameObject;

                c.transform.parent = this.gameObject.transform;
                crosses.Add(c);
            }
        }

    }

    void Update()
    {
        if(isDoing)
        {
            v = Easing.easeInQuad((float)curTime / (float)duration);
            //this.transform.localScale = initialScale + v * scale;
            for(int i = 0; i < gridNum.x; i++)
            {
                for(int j = 0; j < gridNum.y; j++)
                {
                    var s = crosses[(int)(j + i * gridNum.x)].transform.GetChild(0).transform.localScale;
                    crosses[(int)(j + i * gridNum.x)].transform.GetChild(0).transform.localScale = new Vector3(0.1f, (initialScale.y + v * scale), s.z);
                    s = crosses[(int)(j + i * gridNum.x)].transform.GetChild(1).transform.localScale;
                    crosses[(int)(j + i * gridNum.x)].transform.GetChild(1).transform.localScale = new Vector3((initialScale.x + v * scale), 0.1f, s.z); 
                }
            }

            if(isUp)
            {
                curTime++;
                if(curTime > duration) isUp = false;
            } else if(!isUp)
            {
                curTime = curTime - 2;
                if(curTime < 0) isDoing = false;
            }

            if(isRotate)
            {
                for(int i = 0; i < gridNum.x; i++)
                {
                    for(int j = 0; j < gridNum.y; j++)
                    {
                        crosses[(int)(j + i * gridNum.x)].transform.GetChild(0).transform.localEulerAngles += new Vector3(0.0f, 0.0f, curTime);
                        crosses[(int)(j + i * gridNum.x)].transform.GetChild(1).transform.localEulerAngles += new Vector3(0.0f, 0.0f, curTime);
                    }
                }
            }


        } else
        {
            curTime = 0;
            v = 0;
            for(int i = 0; i < gridNum.x; i++)
            {
                for(int j = 0; j < gridNum.y; j++)
                {
                    crosses[(int)(j + i * gridNum.x)].transform.GetChild(0).transform.localScale = initialScale;
                    crosses[(int)(j + i * gridNum.x)].transform.GetChild(1).transform.localScale = initialScale;
                }
            }
            //this.transform.localScale = initialScale;
            //if(Input.GetKeyDown(KeyCode.W) || OscData.kick == 1.0)
            if(Input.GetKeyDown(KeyCode.W) || OscData.kick == 1.0)
            {
                isUp = true;
                isDoing = true;
            }
        }
    }
}
