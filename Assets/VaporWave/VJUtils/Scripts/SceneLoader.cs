using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    SceneLoader instance;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        } else
        {
            instance = this.GetComponent<SceneLoader>();   
        }

        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
       
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("VaporWave");
        }else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("ALife");
        } else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            SceneManager.LoadScene("GPUPolygonTrail");
        } else if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            SceneManager.LoadScene("GPUCollisionParticle");
        }
    }
}
