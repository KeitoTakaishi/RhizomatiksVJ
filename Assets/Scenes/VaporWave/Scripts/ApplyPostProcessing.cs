using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaporWavePostPro : MonoBehaviour
{
    [SerializeField] Material[] material;
    Material mat;


    void Start()
    {
        mat = material[0];
    }

    void Update()
    {
        
        if(MidiReciever.notes[1])
        {
            mat = material[0];
        }else if(MidiReciever.notes[2])
        {
            //Glitch
            mat = material[1];
        }else if(MidiReciever.notes[3])
        {
            //Distortion
            mat = material[2];
        } else if(MidiReciever.notes[4])
        {
            //Distortion
            mat = material[3];
        }
        

    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        //Debug.Log("post");
        Graphics.Blit(src, dst, mat);
    }
}
