using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMoving01 : MonoBehaviour
{
    public float topY = 2000.0f;
    public float bottomY = -2000.0f;
    public float speed = 1.0f;
    public Vector3 rotate;
    void Start()
    {
        
    }

  
    void Update()
    {
        if(transform.position.y > topY)
        {
            this.transform.position = new Vector3(0.0f, bottomY, 0.0f);
        }
        this.transform.transform.position += new Vector3(0.0f, speed, 0.0f);

        float t = Time.realtimeSinceStartup;

        this.transform.eulerAngles = new Vector3(t * rotate.x, t * rotate.y, t * rotate.z);
    }
}
