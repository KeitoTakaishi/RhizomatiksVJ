using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlifeSceneManager : MonoBehaviour
{
     /*
     * 1.reaction diffusion開始 
     * 2.scanもう一度走らせる
     */


    #region gameobject data
    [SerializeField] GameObject grayScottSimurator;
    [SerializeField] GameObject Boids;
    bool isBoid = false;
    #endregion

    #region RenderTexture
    [SerializeField] RenderTexture[] rt;
    #endregion

    void ResetParameters()
    {
        for(int i = 0; i < rt.Length; i++)
        {
            rt[i].Release();
            rt[i] = new RenderTexture(1024, 1024, 1, RenderTextureFormat.ARGBFloat);
            rt[i].Create();

        }
        grayScottSimurator.SetActive(false);
    }

    void Start()
    {
        ResetParameters();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            grayScottSimurator.SetActive(true);
        }else if(Input.GetKeyDown(KeyCode.E))
        {
            //isBoid = !isBoid;
            Boids.SetActive(true);
        }
    }
}
