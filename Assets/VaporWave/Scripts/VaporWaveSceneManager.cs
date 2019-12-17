using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaporWaveSceneManager : MonoBehaviour
{
    /*
     * 1. Barcode
     * 2. FloorEffect
     * 3. Log
     * 4. ring mesh
     * 5. TriangleMesh
     */

    #region gameobject data
    [SerializeField] private GameObject gpuTrailBarcode;
    [SerializeField] private GameObject floor;
    [SerializeField] private GameObject[] log;
    [SerializeField] private GameObject ring;
    [SerializeField] private GameObject triangleMeshParticleSystem;
    #endregion

    #region material data
    [SerializeField] private Material floorDefault;
    [SerializeField] private Material floorEffect;
    #endregion

    #region mode
    int floorMaterialType = 0;
    int triangleMeshParticleSystemActive = 0;
    #endregion

    void ResetParames()
    {
        gpuTrailBarcode.SetActive(false);
        floor.GetComponent<MeshRenderer>().material = floorDefault;
        triangleMeshParticleSystem.SetActive(false);
    }

    void Start()
    {
        ResetParames();
    }

    void Update()
    {

        //BarcodeBoids
        if(Input.GetKeyDown(KeyCode.Q))
        {
            gpuTrailBarcode.SetActive(true);
        }
        //AudioReactiveFloor
        else if(Input.GetKeyDown(KeyCode.W))
        {
            floorMaterialType = (floorMaterialType + 1) % 2;
            if(floorMaterialType == 0)
            {
                floor.GetComponent<MeshRenderer>().material = floorDefault;
            } else
            {
                floor.GetComponent<MeshRenderer>().material = floorEffect;
            }
            
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            int index = (int)Random.Range(0, log.Length);
            Instantiate(log[index]);
        } 
        else if(Input.GetKeyDown(KeyCode.R))
        {
            ring.SetActive(true);
        } else if(Input.GetKeyDown(KeyCode.T))
        {
            triangleMeshParticleSystemActive = (triangleMeshParticleSystemActive + 1) % 2;
            if(triangleMeshParticleSystemActive == 0)
            {
                triangleMeshParticleSystem.SetActive(false);
            } else
            {
                triangleMeshParticleSystem.SetActive(true);
            }
        }
    }
}
