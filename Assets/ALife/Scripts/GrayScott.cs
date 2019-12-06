﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrayScott : MonoBehaviour
{
    [SerializeField] float dx = 0.01f;
    [SerializeField] float dt = 1f;
    [SerializeField] float feed = 0.04f;
    [SerializeField] float kill = 0.06f;

    float Du = 2e-5f;
    float Dv = 1e-5f;

    [SerializeField] RenderTexture u;
    [SerializeField] RenderTexture v;
    [SerializeField] ComputeShader cs;
    int kernel;
    const int TexSize = 256;
    int thread_size_x, thread_size_y;


    void Start()
    {
        InitRenderTexture();
        thread_size_x = 8;
        thread_size_y = 8;
        Init();
        /*
        cs.SetFloat("Dv", Dv);
        cs.SetFloat("Du", Du);
        cs.SetFloat("feed", feed);
        cs.SetFloat("kill", kill);
        cs.SetFloat("dt", dt);
        */

    }

    void Update()
    {
        Simulate();
    }


    //-----------------------------------------------------------
    void InitRenderTexture()
    {
        u.Release();
        u.enableRandomWrite = true;
        u.format = RenderTextureFormat.ARGBFloat;
        u.Create();
        v.Release();
        v.enableRandomWrite = true;
        v.format = RenderTextureFormat.ARGBFloat;
        v.Create();
    }
    //-----------------------------------------------------------
    void Init()
    {
        kernel = cs.FindKernel("init");
        cs.SetTexture(kernel, "u", u);
        cs.SetTexture(kernel, "v", v);
        cs.Dispatch(kernel, TexSize / thread_size_x, TexSize / thread_size_y, 1);
        kernel = cs.FindKernel("simulate");
    }
    //-----------------------------------------------------------
    void Simulate()
    {
        cs.SetTexture(kernel, "u", u);
        cs.SetTexture(kernel, "v", v);
        cs.Dispatch(kernel, TexSize / thread_size_x, TexSize / thread_size_y, 1);
    }

}