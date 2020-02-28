using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbletonRelatedCamera : MonoBehaviour
{
    [SerializeField] List<Material> materials;
    Material mat;
    void Start()
    {
        mat = materials[0];
    }

    void Update()
    {


        mat = materials[0];
        //ScanLine
        if(OscData.brightWhiteNoise == 1)
        {
            mat = materials[1];
            mat.SetFloat("_brightWhiteNoise", OscData.brightWhiteNoise);
        }
        if(OscData.cawbel == 1)
        {
            mat = materials[2];
            mat.SetFloat("_cawbel", OscData.cawbel);
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        //Debug.Log("post");
        Graphics.Blit(src, dst, mat);
    }
}
