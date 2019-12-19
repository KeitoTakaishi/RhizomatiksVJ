using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MidiPreview : MonoBehaviour
{
    public GameObject[] textObj;
    Text[] text;
    bool isVisible = true;

    void Start()
    {
        text = new Text[textObj.Length];
        for(int i = 0; i < text.Length; i++)
        {
            text[i] = textObj[i].GetComponent<Text>();
        }
    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.P))
        {
            isVisible = !isVisible;
        }


        for(int i = 0; i < MidiReciever.knobs.Length; i++)
        {
            textObj[i].SetActive(isVisible);
            text[i].text = i.ToString()+": " + MidiReciever.knobs[i].ToString();
        }    
    }
}
