using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelEmitGPUParticle : MonoBehaviour
{
    [SerializeField] GameObject modelMesh;
    [SerializeField] [Tooltip("Life Min Max")]Vector2 life;//min max
    Mesh cacheMesh;


    #region ComputeShader
    [SerializeField] ComputeShader cs;
    const int BLOCK_SIZE = 64;
    ComputeBuffer positionBuffer;
    ComputeBuffer EmitPosBuffer;
    ComputeBuffer LifeBuffer;
    int kernel;
    int threadGroupSize;
    #endregion

    #region instancingParams
    ComputeBuffer argsBuffer;
    private uint[] args = new uint[5];
    int instancingCount;
    [SerializeField] Mesh srcMesh;
    [SerializeField] Material instancingMat;
    public bool isUpdate;
    #endregion

    void Start()
    {
        cacheMesh = modelMesh.GetComponent<MeshFilter>().sharedMesh;
        InitInstancingParameter();
        CreateBuffer();
        InitBuffer();
        kernel = cs.FindKernel("Init");
        cs.SetBuffer(kernel, "positionBuffer", positionBuffer);
        cs.SetBuffer(kernel, "emitPosBuffer", EmitPosBuffer);
        cs.SetBuffer(kernel, "lifeBuffer", LifeBuffer);
        cs.Dispatch(kernel, Mathf.CeilToInt(instancingCount / BLOCK_SIZE), 1, 1);

    }

    void Update()
    {
        if(isUpdate)
        {
            kernel = cs.FindKernel("Update");
            cs.SetBuffer(kernel, "positionBuffer", positionBuffer);
            cs.SetBuffer(kernel, "emitPosBuffer", EmitPosBuffer);
            cs.SetBuffer(kernel, "lifeBuffer", LifeBuffer);
            cs.SetFloat("dt", Time.deltaTime);
            cs.SetFloat("time", Time.realtimeSinceStartup);

            if(OscData.kick == 1.0f)
            {
                cs.SetFloat("power", Random.Range(1.0f, 2.0f));
            } else
            {
                cs.SetFloat("power", 0.05f);
            }

            cs.Dispatch(kernel, Mathf.CeilToInt(instancingCount / BLOCK_SIZE), 1, 1);

            instancingMat.SetBuffer("positionBuffer", positionBuffer);
            instancingMat.SetBuffer("lifeBuffer", LifeBuffer);
            Graphics.DrawMeshInstancedIndirect(srcMesh, 0, instancingMat,
            new Bounds(Vector3.zero, Vector3.one * 32.0f), argsBuffer);
        }
    }
    /*
    * GPU Instancing-------------------------------------------
    */
    private void InitInstancingParameter()
    {
        instancingCount = cacheMesh.vertexCount;
        Debug.Log(instancingCount);
        argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
        args[0] = srcMesh.GetIndexCount(0);
        args[1] = (uint)instancingCount;
        args[2] = srcMesh.GetIndexStart(0);
        args[3] = srcMesh.GetBaseVertex(0);
        args[4] = 0;
        argsBuffer.SetData(args);
    }


    /*
    * GPU Instancing-------------------------------------------
    */
    private void CreateBuffer()
    {
        positionBuffer = new ComputeBuffer(instancingCount, sizeof(float) * 3);
        EmitPosBuffer = new ComputeBuffer(instancingCount, sizeof(float) * 3);
        LifeBuffer = new ComputeBuffer(instancingCount, sizeof(float) * 3);

    }

    //---------------------------------------------------------
    private void InitBuffer()
    {
        var posList = new List<Vector3>();
        var emitPosList = new List<Vector3>();
        var lifeList = new List<Vector3>();

        for(int i = 0; i < instancingCount; i++)
        {
            
            posList.Add(Vector3.zero);
            //Debug.Log(cacheMesh.vertices[i] * 10.0f);
            var p = cacheMesh.vertices[i] * 50.0f;
            p = new Vector3(p.x, p.y, p.z * -1.0f);
            emitPosList.Add(p);
            lifeList.Add(new Vector3(0, Random.Range(life.x, life.y), 0f));
            
        }

        positionBuffer.SetData(posList);
        EmitPosBuffer.SetData(emitPosList);
        LifeBuffer.SetData(lifeList);
    }
    //---------------------------------------------------------
    private void OnDisable()
    {
        if(positionBuffer == null)
        {
            positionBuffer.Release();
        }

        if(EmitPosBuffer == null)
        {
            EmitPosBuffer.Release();
        }
        if(LifeBuffer == null)
        {
            LifeBuffer.Release();
        }
        if(argsBuffer == null)
        {
            argsBuffer.Release();
        }
    }

    //---------------------------------------------------------
    private void OnDestroy()
    {
        if(positionBuffer == null)
        {
            positionBuffer.Release();
        }

        if(EmitPosBuffer == null)
        {
            EmitPosBuffer.Release();
        }

        if(LifeBuffer == null)
        {
            LifeBuffer.Release();
        }

        if(argsBuffer == null)
        {
            argsBuffer.Release();
        }
    }
}
