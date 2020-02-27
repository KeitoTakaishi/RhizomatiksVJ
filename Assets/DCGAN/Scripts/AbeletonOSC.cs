using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbeletonOSC : MonoBehaviour
{

    [SerializeField] List<Material> materials;
    
    void Start()
    {
        //materials = new List<Material>();
    }

    void Update()
    {
        //print(OscData.brightWhiteNoise);
        materials[0].SetFloat("brightWhiteNoise", OscData.brightWhiteNoise);
       
    }
}
