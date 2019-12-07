using UnityEngine;
using MidiJack;

public class MidiReciever : MonoBehaviour
{
    public bool[] notes;




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
        //Debug.Log("NoteOff: " + channel + "," + note);
    }

    void Knob(MidiChannel channel, int knobNumber, float knobValue)
    {

        switch(knobNumber)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                
                break;
            case 4:
                
                break;
            case 5:
               
                break;
            case 6:
                break;

            default:
                break;
        }


       //Debug.Log("Knob: " + knobNumber + "," + knobValue);
    }

    void OnEnable()
    {
        MidiMaster.noteOnDelegate += NoteOn;
        MidiMaster.noteOffDelegate += NoteOff;
        MidiMaster.knobDelegate += Knob;

        notes = new bool[8];
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
