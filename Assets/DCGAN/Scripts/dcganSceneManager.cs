using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dcganSceneManager : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] dcgan_gpuparticle gpuparticle;
    bool camBG = false;
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            camBG = !camBG;
            if(camBG)
            {
                cam.clearFlags = CameraClearFlags.Skybox;
            } else
            {
                cam.clearFlags = CameraClearFlags.SolidColor;
            }
        }else if(Input.GetKeyDown(KeyCode.W))
        {
            gpuparticle.calcFlag = !gpuparticle.calcFlag;
        }
    }
}
