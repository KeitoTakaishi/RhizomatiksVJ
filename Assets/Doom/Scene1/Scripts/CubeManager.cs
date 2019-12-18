using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    List<GameObject> cubes;
    [SerializeField] GameObject parentObj;
    public Vector3 RotateSpeed;
    public Shader shader;
    void Start()
    {
        cubes = new List<GameObject>();
        foreach(Transform childTransform in parentObj.transform)
        {
            GameObject child = childTransform.gameObject;
            child.GetComponent<MeshRenderer>().material = new Material(shader);
            cubes.Add(child);
        }
    }
    float time;
    void Update()
    {
        time = Time.realtimeSinceStartup * RotateSpeed.y;
       for(int i = 0; i < cubes.Count; i++)
        {
            cubes[i].transform.localEulerAngles = new Vector3(time * RotateSpeed.x, time + i * 15.0f, RotateSpeed.z);
        }
    }
}
