using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaporwaveAudioReactionManager : MonoBehaviour
{

    #region gameobject
    [SerializeField]
    GameObject[] column;
    #endregion

    #region data
    List<AudioReactiveScale> audioReactiveScaleList; 
    #endregion


    void Start()
    {
        audioReactiveScaleList = new List<AudioReactiveScale>();
        for(int i = 0; i < column.Length; i++)
        {
            audioReactiveScaleList.Add ( column[i].GetComponent<AudioReactiveScale>());
            audioReactiveScaleList[i].Scale = new Vector3(0, Random.Range(5.0f, 10.0f), 0);
        }
    }

    void Update()
    {
        
    }
}
