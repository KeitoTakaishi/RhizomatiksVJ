using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    SceneLoader instance;

    [SerializeField] KeyCode[] sceneKey;
    

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
        Cursor.visible = false;
    }

    void Update()
    {
        
        //if(Input.GetKeyDown(sceneKey[0]))
        if(OscData.scene == 1 || Input.GetKeyDown(sceneKey[0]))
        {
            ///SceneManager.LoadScene("VaporWave");
            SceneManager.LoadScene("DCGAN");
        }
        //else if(Input.GetKeyDown(sceneKey[1]))
        else if(OscData.scene == 2 || Input.GetKeyDown(sceneKey[1]))
        {
            SceneManager.LoadScene("ALife");
        }
        //else if(Input.GetKeyDown(sceneKey[2]))
         else if(OscData.scene == 3)
        {
            SceneManager.LoadScene("GPUPolygonTrail");
        }
        //else if(Input.GetKeyDown(sceneKey[3]))
         else if(OscData.scene == 4)
        {
            SceneManager.LoadScene("GPUCollisionParticle");
        }
         //else if(Input.GetKeyDown(sceneKey[4]))
         else if(OscData.scene == 5)
        {
            SceneManager.LoadScene("Doom");
        }

        OscData.scene = 0;
    }
}
