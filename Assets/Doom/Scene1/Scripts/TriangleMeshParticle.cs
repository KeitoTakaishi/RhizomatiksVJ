using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Linq;

public class TriangleMeshParticle : MonoBehaviour
{
    #region inspector
    [SerializeField] int _triangleCount = 100;

    public int triangleCount
    {
        get { return _triangleCount; }
        set { _triangleCount = value; }
    }
    #endregion



    #region private
    bool isCompute = true;
    [SerializeField]ComputeShader cs;
    Mesh mesh;
    [SerializeField]Material material;
    MaterialPropertyBlock props;
    ComputeBuffer drawArgsBuffer;
    ComputeBuffer positionBuffer;
    ComputeBuffer normalBuffer;
    ComputeBuffer lifeBuffer;
    [SerializeField] float maxLife = 5.0f;
    [SerializeField] float noiseScale = 0.2f;
    [SerializeField] float positionScale = 0.2f;
    [SerializeField] Vector4 force = new Vector4(0.0f, 0.1f, 0.0f, 0.0f);
    [SerializeField] Vector4 offSet = new Vector4(0.0f, 2500f, 0.0f, 0.0f);
    #endregion


    #region Compute configurations
    const int kThreadCount = 64;
    int ThreadGroupCount { get { return _triangleCount / kThreadCount; } }
    int TriangleCount { get { return kThreadCount * ThreadGroupCount; } }
    #endregion



    void Start()
    {
        // Mesh with single triangle.
        mesh = new Mesh();
        mesh.vertices = new Vector3[3];
        mesh.SetIndices(new[] { 0, 1, 2 }, MeshTopology.Triangles, 0);
        mesh.UploadMeshData(true);

        drawArgsBuffer = new ComputeBuffer(1, 5 * sizeof(uint), ComputeBufferType.IndirectArguments);

        material = new Material(material);
        props = new MaterialPropertyBlock();

      
    }

    
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.C))
        {
            //isCompute = !isCompute;
        }
        if(isCompute)
        {
            if(positionBuffer == null || positionBuffer.count != TriangleCount * 3)
            {
                Debug.Log("Buffer Release");
                if(positionBuffer != null) positionBuffer.Release(); //三角形の個数が変わった場合
                if(normalBuffer != null) normalBuffer.Release();


                positionBuffer = new ComputeBuffer(TriangleCount * 3, Marshal.SizeOf(typeof(Vector4)));
                normalBuffer = new ComputeBuffer(TriangleCount * 3, Marshal.SizeOf(typeof(Vector4)));
                lifeBuffer = new ComputeBuffer(TriangleCount, Marshal.SizeOf(typeof(float)));

                //var life = Enumerable.Repeat(Random.Range(0.0f, maxLife), TriangleCount).ToList();
                var life = new List<float>();
                for(int i = 0; i < TriangleCount; i++)
                {
                    life.Add(Random.Range(0, maxLife));
                }
                lifeBuffer.SetData(life.ToArray());

                //mesh`s indexCount, instanceCount
                drawArgsBuffer.SetData(new uint[5] { mesh.GetIndexCount(0), (uint)TriangleCount, 0, 0, 0 });
            }



            var kernel = cs.FindKernel("Update");
            cs.SetBuffer(kernel, "PositionBuffer", positionBuffer);
            cs.SetBuffer(kernel, "NormalBuffer", normalBuffer);
            cs.SetBuffer(kernel, "LifeBuffer", lifeBuffer);
            cs.SetFloat("Time", Time.time);
            cs.SetFloat("maxLife", maxLife);
            cs.SetFloat("noiseScale", noiseScale);
            cs.SetFloat("positionScale", positionScale);
            cs.SetVector("force", force);
            //cs.SetVector("offSet", offSet);
            cs.Dispatch(kernel, ThreadGroupCount, 1, 1);

            material.SetBuffer("PositionBuffer", positionBuffer);
            material.SetBuffer("NormalBuffer", normalBuffer);
            material.SetBuffer("LifeBuffer", lifeBuffer);

            Graphics.DrawMeshInstancedIndirect(mesh, 0, material,
                new Bounds(transform.position, transform.lossyScale * 5)
                , drawArgsBuffer, 0, props);
        } else
        {
            if(positionBuffer != null)
            {
                positionBuffer.Release();
                positionBuffer = null;
            }

            if(normalBuffer != null)
            {
                normalBuffer.Release();
                normalBuffer = null;
            }

            if(lifeBuffer != null)
            {
                lifeBuffer.Release();
                lifeBuffer = null;
            }
        }


    }

    private void OnDisable()
    {
        if(positionBuffer != null)
        {
            positionBuffer.Release();
            positionBuffer = null;
        }

        if(normalBuffer != null)
        {
            normalBuffer.Release();
            normalBuffer = null;
        }

        if(lifeBuffer != null)
        {
            lifeBuffer.Release();
            lifeBuffer = null;
        }
    }


    private void OnDestroy()
    {
        Debug.Log("Buffer Release Destroy");
        if(positionBuffer != null)
        {
            positionBuffer.Release();
            positionBuffer = null;
        }

        if(normalBuffer != null)
        {
            normalBuffer.Release();
            normalBuffer = null;
        }

        if(lifeBuffer != null)
        {
            lifeBuffer.Release();
            lifeBuffer = null; 
        }
    }
}
