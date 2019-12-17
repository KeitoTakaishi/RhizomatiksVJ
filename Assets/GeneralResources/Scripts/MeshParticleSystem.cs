using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshParticleSystem : MonoBehaviour
{
    [SerializeField] GameObject triangleMesh;
    [SerializeField] int duration = 20;
    int time = 0;
    void Start()
    {
            
    }

    void Update()
    {
        if(time > duration)
        {
            if(OscData.rythm == 1)
            {
                Instantiate(triangleMesh);
            }
            time = 0;
        }
        time++;
    }

}
