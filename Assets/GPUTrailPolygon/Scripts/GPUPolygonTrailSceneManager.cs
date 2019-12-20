using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUPolygonTrailSceneManager : MonoBehaviour
{
    [SerializeField] GameObject GpuPolygonTrail;
    [SerializeField] GameObject crossesObj;
    AudioReactiveCrossScale cross;

    [SerializeField] GameObject[] particleSystem;

    void ResetParameters()
    {
        GpuPolygonTrail.SetActive(false);
    }


    void Start()
    {
        ResetParameters();
        cross = crossesObj.GetComponent<AudioReactiveCrossScale>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            GpuPolygonTrail.SetActive(true);
        } else if(Input.GetKeyDown(KeyCode.W))
        {
            cross.isRotate = true;
        } else if(Input.GetKeyDown(KeyCode.E))
        {
            particleSystem[0].SetActive(true);
        }
    }
}
