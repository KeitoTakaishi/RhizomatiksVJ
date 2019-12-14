using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReactiveVertex : MonoBehaviour
{
    [SerializeField] Material material;

    void Start()
    {
        
    }

    void Update()
    {
        material.SetFloat("_Kick", OscData.kick);
    }
}
