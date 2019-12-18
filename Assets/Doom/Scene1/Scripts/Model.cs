using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        float t = Time.realtimeSinceStartup / 10.0f;
        float r = Mathf.PerlinNoise(t, t) * 360.0f;
        transform.eulerAngles = new Vector3(r*2.0f, r, r);
    }
}
