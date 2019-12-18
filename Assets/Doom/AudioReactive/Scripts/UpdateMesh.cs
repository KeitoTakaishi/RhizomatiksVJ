using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateMesh : MonoBehaviour
{
    [SerializeField] GameObject model;
    private MeshFilter targetMF;
    private MeshFilter mf;
    private MeshRenderer mr;
    private Mesh mesh;
    private List<Vector3> originalVertices, vertices;
    private List<Vector3> originalNormals, normals;
    private List<int> indeies;
    private int vertexCount;
    private int indexCount;
    private int curIndex = 0;
    private int curVertexCount = 0;
    [SerializeField] private float positionScale = 1.0f;
    [SerializeField] private Shader shader;
    private int timeCount = 0;
 
    void Start()
    {
        this.mf = this.GetComponent<MeshFilter>();
        this.mr = this.GetComponent<MeshRenderer>();
        targetMF = model.GetComponent<MeshFilter>();
        originalVertices = new List<Vector3>(targetMF.sharedMesh.vertices);
        for(int i = 0; i < originalVertices.Count; i++)
        {
            originalVertices[i] = originalVertices[i] * positionScale;
        }

        originalNormals = new List<Vector3>(targetMF.sharedMesh.normals);
        this.vertexCount = targetMF.sharedMesh.vertexCount;
        this.indexCount = (int)targetMF.sharedMesh.GetIndexCount(0);
        this.vertices = new List<Vector3>();
        this.normals = new List<Vector3>();
        this.indeies = new List<int>();
        this.mesh = new Mesh();
        //var ind = targetMF.sharedMesh.GetTriangles(0);
        Debug.Log("VertexCount : " + targetMF.sharedMesh.vertexCount);

    }

    void Update()
    {
        if(curIndex < indexCount)
        {
            for(int i = 0; i < 3; i++)
            {
                var index = targetMF.sharedMesh.GetIndices(0)[curIndex + i];
                indeies.Add(index);
            }
            curIndex += 3;
            RebuildMesh();
        } else
        {
            int rnd = 0;
            if(Time.frameCount % 20 == 0)
            {
                rnd = (int)UnityEngine.Random.Range(0f, 2f);
            }
            RebuildMesh(rnd);
            timeCount++;
        }


        if(timeCount > 300)
        {
            timeCount = 0;
            indeies.Clear();
            curIndex = 0;
        }
        

        
    }

    void RebuildMesh()
    {
        mesh.SetVertices(originalVertices);
        mesh.SetNormals(originalNormals);

        if(UnityEngine.Random.Range(0, 2) < 1)
        {
            mesh.SetIndices(indeies.ToArray(), MeshTopology.Triangles, 0);
        } else
        {
            mesh.SetIndices(indeies.ToArray(), MeshTopology.Lines, 0);
        }
        
        mf.sharedMesh = mesh;
    }

    void RebuildMesh(int mode)
    {
        if(mode == 0) {
            mesh.SetIndices(indeies.ToArray(), MeshTopology.Triangles, 0);
         } else {
            mesh.SetIndices(indeies.ToArray(), MeshTopology.Lines, 0);
        }
        mf.sharedMesh = mesh;
    }
}
