using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaporWavePostPro : MonoBehaviour
{
    [SerializeField] Material[] material;
    [SerializeField] GameObject midiController;
    MidiReciever midi;
    Material mat;


    void Start()
    {
        midi = midiController.GetComponent<MidiReciever>();
        mat = material[0];
    }

    void Update()
    {
        if(midi.notes[1])
        {
            mat = material[0];
        }else if(midi.notes[2])
        {
            //Glitch
            mat = material[1];
        }else if(midi.notes[3])
        {
            //Distortion
            mat = material[2];
        } else if(midi.notes[4])
        {
            //Distortion
            mat = material[3];
        }


    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, mat);
    }
}
