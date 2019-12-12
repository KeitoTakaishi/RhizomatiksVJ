using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailTarget : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float z;
    [SerializeField] float r;
    void Start()
    {
        
    }

    void Update()
    {
        float theta = Time.realtimeSinceStartup * Mathf.Deg2Rad * speed;
        Vector3 p = new Vector3(r * Mathf.Cos(theta), r * Mathf.Sin(theta), z);
        this.transform.position = p;
    }
}
