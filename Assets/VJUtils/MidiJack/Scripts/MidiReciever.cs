using UnityEngine;
using MidiJack;

public class MidiReciever : MonoBehaviour
{
    static public bool[] notes;
    static public float[] knobs;


    void NoteOn(MidiChannel channel, int note, float velocity)
    {
        switch(note)
        {
            case 36:
                notes[0] = true;
                break;
            case 37:
                notes[1] = true;
                break;
            case 38:
                notes[2] = true;
                break;
            case 39:
                notes[3] = true;
                break;
            case 40:
                notes[4] = true;
                break;
            case 41:
                notes[5] = true;
                break;
            case 42:
                notes[6] = true;
                break;
            case 43:
                notes[7 ] = true;
                break;
        }

    }

    void NoteOff(MidiChannel channel, int note)
    {
        switch(note)
        {
            case 36:
                notes[0] = false;
                break;
            case 37:
                notes[1] = false;
                break;
            case 38:
                notes[2] = false;
                break;
            case 39:
                notes[3] = false;
                break;
            case 40:
                notes[4] = false;
                break;
            case 41:
                notes[5] = false;
                break;
            case 42:
                notes[6] = false;
                break;
            case 43:
                notes[7] = false;
                break;
        }
    }

    void Knob(MidiChannel channel, int knobNumber, float knobValue)
    {
        //Debug.Log("knobNumber" + knobNumber);
        //Debug.Log("knobNumber" + knobNumber);
        //switch(knobNumber)
        switch(knobNumber)
        {
            case 0:
                knobs[0] = knobValue;
               //Debug.Log("0 : " + knobs[0]);
                break;
            case 1:
                knobs[1] = knobValue;
                //Debug.Log("1 : " + knobs[1]);
                break;
            case 2:
                knobs[2] = knobValue;
                //Debug.Log("2 : " + knobs[2]);
                break;
            case 3:
                knobs[3] = knobValue;
                //Debug.Log("3 : " + knobs[3]);
                break;
            case 4:
                knobs[4] = knobValue;
                //Debug.Log("4 : " + knobs[4]);
                break;
            case 5:
               
                knobs[5] = knobValue;
                //Debug.Log("5 : " + knobs[5]);
                break;
            case 6:
               
                knobs[6] = knobValue;
                //Debug.Log("6 : " + knobs[6]);
                break;
            case 7:
   
                knobs[7] = knobValue;
                //Debug.Log("7 : " + knobs[7]);
                break;
            default:
                break;
        }
    }

    void OnEnable()
    {
        MidiMaster.noteOnDelegate += NoteOn;
        MidiMaster.noteOffDelegate += NoteOff;
        MidiMaster.knobDelegate += Knob;

        notes = new bool[8];
        knobs = new float[8];
        //camAnim = cam.GetComponent<CameraAnimation>();
        //behave = cam.GetComponent<PostProcessingBehaviour>();
        //bloomSetting = behave.profile.bloom.settings;
        //colorGradientSetting = behave.profile.colorGrading.settings;
    }

    void OnDisable()
    {
        MidiMaster.noteOnDelegate -= NoteOn;
        MidiMaster.noteOffDelegate -= NoteOff;
        MidiMaster.knobDelegate -= Knob;
    }
}
