using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDebug : MonoBehaviour
{
    [SerializeField] Mesh mesh;

    void Start()
    {
        var indexCount = mesh.GetIndexCount(0);
        Debug.Log("indexCount" + indexCount.ToString());
        Debug.Log("---------------");
        var indexArr = mesh.GetIndices(0);
        for(int i = 0; i < indexArr.Length; i++)
        {
            Debug.Log(i.ToString() + " : " + indexArr[i].ToString());
        }
        
    }

    void Update()
    {
        
    }
}
