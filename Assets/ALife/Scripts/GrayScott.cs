using System.Collections;
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
    const int TexSize = 1024;
    int thread_size_x, thread_size_y;


    void Start()
    {
        InitRenderTexture();
        thread_size_x = 32;
        thread_size_y = 32;
        Init();
        /*
        cs.SetFloat("Dv", Dv);
        cs.SetFloat("Du", Du);
        cs.SetFloat("feed", feed);
        cs.SetFloat("kill", kill);
        cs.SetFloat("dt", dt);
        */

    }

    private void OnEnable()
    {
        InitRenderTexture();
        thread_size_x = 32;
        thread_size_y = 32;
        Init();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W)){
            //Init();
            RandomSet();
        }
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


        Du = 2e-5f;
        Dv = 1e-5f;
        dt = 1.0f;
        dx = 0.01f;
        SendParames(Du, Dv, dt, dx);
        

        cs.SetTexture(kernel, "u", u);
        cs.SetTexture(kernel, "v", v);
        cs.SetFloat("TexSize", u.width);
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

    //-----------------------------------------------------------
    void RandomSet()
    {
        kernel = cs.FindKernel("randomSet");
        cs.SetFloat("time", Time.realtimeSinceStartup);
        cs.SetTexture(kernel, "u", u);
        cs.SetTexture(kernel, "v", v);
        SendParames(Du, Dv, dt, dx);
        cs.Dispatch(kernel, TexSize / thread_size_x, TexSize / thread_size_y, 1);
        kernel = cs.FindKernel("simulate");
    }

    //-----------------------------------------------------------
    void SendParames(float _Du, float _Dv, float _dt, float _dx)
    {
        float _feed = 0.0f;
        float _kill = 0.0f;

        float index = Random.Range(0.0f, 1.0f);
        if(index < 0.2f)
        {
            feed = 0.04f;
            kill = 0.06f;
        } else if(0.2f <= index && index < 0.4f)
        {
            feed = 0.035f;
            kill = 0.065f;
        } else if(0.4f <= index && index < 0.6f)
        {
            feed = 0.012f;
            kill = 0.05f;
        } else if(0.6f <= index && index < 0.8f)
        {
            feed = 0.025f;
            kill = 0.05f;
        } else if(0.8f <= index)
        {
            feed = 0.022f;
            kill = 0.051f;
        }

        cs.SetFloat("Du", _Du);
        cs.SetFloat("Dv", _Du);
        cs.SetFloat("feed", _feed);
        cs.SetFloat("kill", _kill);
        cs.SetFloat("dt", _dt);
        cs.SetFloat("dx", _dx);
    }

}
