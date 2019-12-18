using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstSection : MonoBehaviour
{
    public Material solidColor;


    void Start()
    {
        RenderSettings.skybox = solidColor;
    }

    void Update()
    {
        
    }
}
