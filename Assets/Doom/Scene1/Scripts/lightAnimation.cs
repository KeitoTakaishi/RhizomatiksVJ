using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightAnimation : MonoBehaviour
{
    public GameObject light0;
    public GameObject light1;
    [SerializeField][Range(0.0f, 10.0f)]
    float rotationSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        float r = Time.deltaTime * rotationSpeed;
        light0.transform.Rotate(new Vector3(r, r, r));
        light1.transform.Rotate(new Vector3(r, r*0.5f, r*0.5f));
    }
}
