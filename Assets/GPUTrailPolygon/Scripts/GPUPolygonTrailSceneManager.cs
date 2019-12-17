using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUPolygonTrailSceneManager : MonoBehaviour
{
    [SerializeField] GameObject GpuPolygonTrail;


    void ResetParameters()
    {
        GpuPolygonTrail.SetActive(false);
    }


    void Start()
    {
        ResetParameters();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            GpuPolygonTrail.SetActive(true);
        }    
    }
}
