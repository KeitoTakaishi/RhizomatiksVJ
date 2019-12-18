using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager1 : MonoBehaviour
{
    public GameObject doom;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!doom.activeSelf)
            {
                doom.SetActive(true);
            }
        }
    }
}
