using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twist : MonoBehaviour
{
    [SerializeField] Material twistMaterial;
    Vector2 screeSize;
    [SerializeField] Vector2 center = new Vector2(0, 0);
    [SerializeField] float radius =1.0f;
    [SerializeField] float strength = 10.0f;


    void Start()
    {
        
    }

    void Update()
    {
        screeSize = new Vector2(Screen.width, Screen.height);
        twistMaterial.SetVector("Size_Center", new Vector4(screeSize.x, screeSize.y, center.x, center.y));
        twistMaterial.SetFloat("radius", radius); 
        twistMaterial.SetFloat("strength", strength);
    }
}
