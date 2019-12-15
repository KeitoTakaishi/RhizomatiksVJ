using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineEffect : MonoBehaviour
{ 
    enum StartPointMode
    {
        constant = 0,
        circle = 1,

    }


    List<LineRenderer> lines;
    [SerializeField] ModifyMesh modifyMesh;
    int num;
    [SerializeField]
    StartPointMode mode;

  

    #region circleModeParameters
    [SerializeField] float radius = 3.0f;
    [SerializeField] float offSetCoef = 30.0f;
    [SerializeField] float speed = 10.0f;
    float theta = 0.0f;
    #endregion

    void Start()
    {
        mode = new StartPointMode();
        mode = StartPointMode.circle;

        
        num = transform.childCount;
        lines = new List<LineRenderer>();
        for(int i = 0; i < num; i++)
        {
            lines.Add(transform.GetChild(i).gameObject.GetComponent<LineRenderer>());
            lines[i].SetWidth(0.01f, 0.01f);
            lines[i].SetVertexCount(2);
        }
    }

    void Update()
    {
        for(int i = 0; i < num; i++)
        {
            if(mode == StartPointMode.constant)
            {
                lines[i].SetPosition(0, Vector3.zero);
                lines[i].SetPosition(1, modifyMesh.points[i]);


            }else if(mode == StartPointMode.circle)
            {
               var startPos = new Vector3(radius * Mathf.Sin(speed * (Time.realtimeSinceStartup + i * offSetCoef) * Mathf.Deg2Rad),
                                          0.0f,
                                          radius * Mathf.Cos(speed * (Time.realtimeSinceStartup + i * offSetCoef) * Mathf.Deg2Rad));


                lines[i].SetPosition(0, startPos);
                lines[i].SetPosition(1, modifyMesh.points[i]);
            }
        }

    }
}
