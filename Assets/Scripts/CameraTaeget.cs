using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTaeget : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] float speed;
    Vector3 startPos;
    void Start()
    {
        startPos = this.transform.position;
    }

    void Update()
    {
        float t = Time.realtimeSinceStartup;
        this.transform.position = new Vector3(
            radius * Mathf.Cos(speed * t * Mathf.Deg2Rad),
            0.0f,
            radius * Mathf.Sin(speed * t * Mathf.Deg2Rad)) + startPos;
    }
}
