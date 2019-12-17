using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorEffect : MonoBehaviour
{
    [SerializeField] Material floorEffect;
    [SerializeField] float amp;
    void Start()
    {
        
    }

    void Update()
    {
        if(OscData.kick == 1)
        {
            amp = Random.Range(1.0f, 15.0f);
        }
        floorEffect.SetFloat("low", OscData.low * amp);
    }
}
