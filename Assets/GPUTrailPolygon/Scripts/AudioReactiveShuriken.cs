using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReactiveShuriken : MonoBehaviour
{

    public ParticleSystem shuriken;

    void Start()
    {
        if(shuriken == null)
        {
            shuriken = this.GetComponent<ParticleSystem>();
        }
    }

    void onEnable()
    {
        if(shuriken == null)
        {
            shuriken = this.GetComponent<ParticleSystem>();
        }
    }

    void Update()
    {
        //shuriken.startRotation = Random.RandomRange(0.0f, 180f.0);
        if(OscData.kick == 1.0)
        {
            //shuriken.Emit(Vector3.zero, new Vector3(0.0f, 1.0f, 0.0f), 1.0f, 1.0f, Color.white);
            shuriken.Play();
        }

    }
}
