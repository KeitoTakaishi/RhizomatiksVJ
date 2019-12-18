using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopLayerShaderSelecter : MonoBehaviour
{
    [SerializeField] Image img;
    [SerializeField] Material[] filter;
    [SerializeField]int interval;
    int index;
    void Start()
    {
        index = 0;
    }

    void Update()
    {
        img.material = filter[index];
        if(Time.frameCount % interval == 0)
        {
            Debug.Log(index);
            index = ((int)( Random.Range(0, filter.Length))) % filter.Length; 
           
        }
    }
}
