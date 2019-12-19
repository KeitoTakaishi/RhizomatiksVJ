using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUParicleCollisionManager : MonoBehaviour
{
    public Material mat;
    void Start()
    {
        
    }

    void Update()
    {
        mat.SetFloat("kick", OscData.kick);
    }
}
