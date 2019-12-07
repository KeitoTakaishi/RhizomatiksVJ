using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyPostProcessing : MonoBehaviour
{
    [SerializeField] Material material;



    void Start()
    {
    }

    void Update()
    {
        
    }

        void OnRenderImage(RenderTexture src, RenderTexture dst)
        {
        
        Graphics.Blit(src, dst, material);
    }
}
