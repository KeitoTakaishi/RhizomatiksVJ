using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    Camera cam;
    [SerializeField] Material transitionMaterial;
    [SerializeField] Material defaultMaterial;
    Material curMat;
    void Awake()
    {
        curMat = transitionMaterial;
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        StartCoroutine("ChangeFov");
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    IEnumerator ChangeFov()
    {
        for(int i = 10; i >= 0; i--)
        {
            //cam.fielOfView = i;
            transitionMaterial.SetFloat("power", (float)i * 1.0f / 60.0f);
            if(i == 0) curMat = defaultMaterial;
            yield return null;

        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, curMat);
    }
}
