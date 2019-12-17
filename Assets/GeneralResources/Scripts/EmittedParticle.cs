using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmittedParticle : MonoBehaviour
{
    [SerializeField] float life;

    void Start()
    {
        
    }
    private void OnEnable()
    {
        Destroy(gameObject,life);
        this.transform.localScale *= Random.Range(10.0f, 15.0f);
    }

    void Update()
    {
        this.transform.position += new Vector3(0, 1, 0);    
    }
}
