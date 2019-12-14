using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    Camera cam;

    void Awake()
    {
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        StartCoroutine("ChangeFov");
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    IEnumerator ChangeFov()
    {
        for(int i = 0; i <= 60; i++)
        {
            cam.fieldOfView = i;
            yield return null;

        }
    }
}
