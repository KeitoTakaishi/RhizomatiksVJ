using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class RingMesh : MonoBehaviour
{
    #region MeshData
    [SerializeField] float radius;
    [SerializeField] float height;
    [SerializeField] float width;
    [SerializeField] float trailResolution;
    List<Vector3> verticesList;
    List<Vector3> normalList;
    List<Vector2> uvList;
    List<int> indexList;
    [SerializeField] MeshFilter mf;
    [SerializeField] MeshRenderer mr;
    #endregion

    void Start()
    {
        CreateMesh();
    }

    private void OnEnable()
    {
        CreateMesh();
    }

    void Update()
    {
        
    }
    void CreateMesh()
    {
        var mesh = new Mesh();
        verticesList = new List<Vector3>();
        normalList = new List<Vector3>();
        uvList = new List<Vector2>();
        indexList = new List<int>();


        for(int i = 0; i < trailResolution+1; i++)
        {
            //float delta = length / (float)trailResolution;
            float theta = (2.0f* Mathf.PI / (float)(trailResolution-1));
            theta *= (float)i;
            var p1 = new Vector3(radius *  Mathf.Sin(theta), 0, radius * Mathf.Cos(theta));
            var p2 = new Vector3(radius * Mathf.Sin(theta), width, radius *  Mathf.Cos(theta));
            p1 += this.transform.position;
            p2 += this.transform.position;

            verticesList.Add(p1);
            verticesList.Add(p2);
            
            
            normalList.Add(p1.normalized);
            normalList.Add(p2.normalized);

            uvList.Add(new Vector2((float)i / (float)(trailResolution - 1.0), 0.0f));
            uvList.Add(new Vector2((float)i / (float)(trailResolution - 1.0), 1.0f));

            //if(i > 1 && i % 2 == 0)
            if(i > 1)

            {
                /*
                int i0 = i;
                int i1 = i + 1;
                int i2 = i - 1;
                int i3 = i - 2;
                */
                int i0 = i*2;
                int i1 = i*2 + 1;
                int i2 = i*2 - 1;
                int i3 = i*2 - 2;

                indexList.Add(i0);
                indexList.Add(i1);
                indexList.Add(i2);

                indexList.Add(i0);
                indexList.Add(i2);
                indexList.Add(i3);
            }
            
            mesh.vertices = verticesList.ToArray();
            mesh.normals = normalList.ToArray();
            mesh.uv = uvList.ToArray();
            mesh.SetIndices(indexList.ToArray(), MeshTopology.Triangles, 0);
            mesh.RecalculateNormals();
            mf = GetComponent<MeshFilter>();
            mf.mesh = mesh;

        }
    }
}
