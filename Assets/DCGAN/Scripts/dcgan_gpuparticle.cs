using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;


public class dcgan_gpuparticle : MonoBehaviour
{
    struct Parameters
    {
        Vector3 pos;
        Vector2 life; 

        public Parameters(Vector3 p, Vector2 l)
        {
            pos = p;
            life = l;
        }
    };

    #region ComputeShader
    [SerializeField] ComputeShader cs;
    [SerializeField] int instancingCount; //BLOCK_SIZEの倍数
    const int BLOCK_SIZE = 64;
    ComputeBuffer parameterBuffer;
    int kernel;
    int threadGroupSize;
    #endregion

    #region instancingParams
    ComputeBuffer argsBuffer;
    private uint[] args = new uint[5];
    [SerializeField] Mesh srcMesh;
    [SerializeField] Material instancingMat;
    #endregion


    void Start()
    {
        InitInstancingParameter();
        CreateComputeBuffer();
        kernel = cs.FindKernel("init");
    }

    void Update()
    {
        cs.SetFloat("dt", Time.deltaTime);
        cs.SetFloat("time", Time.realtimeSinceStartup);
        cs.SetBuffer(kernel, "parames", parameterBuffer);
        cs.Dispatch(kernel, instancingCount/64, 1, 1);
        instancingMat.SetBuffer("paramsBuffer", parameterBuffer);
        Graphics.DrawMeshInstancedIndirect(srcMesh, 0, instancingMat, new Bounds(Vector3.zero, Vector3.one * 32.0f), argsBuffer);
    }
    //---------------------------------
    //Instancing
    private void InitInstancingParameter()
    {
        argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
        args[0] = srcMesh.GetIndexCount(0);
        args[1] = (uint)instancingCount;
        args[2] = srcMesh.GetIndexStart(0);
        args[3] = srcMesh.GetBaseVertex(0);
        args[4] = 0;
        argsBuffer.SetData(args);
    }

    //---------------------------------
    void CreateComputeBuffer()
    {
        parameterBuffer = new ComputeBuffer(instancingCount, Marshal.SizeOf(typeof(Parameters)));
        Parameters[] parameteres = new Parameters[parameterBuffer.count];
        float n = 128.0f;
        for(int i = 0; i < instancingCount; i++)
        {
            var x = (i % n);
            x = (x * 2.0f - n) / 2.0f;

            var y = ((float)(i) / n);
            y = (y * 2.0f - n) / 2.0f;

            var pos = new Vector2(x, y);


            //var life = new Vector2(Random.Range(1.5f, 3.0f), Random.Range(1.5f, 3.0f));
            var life = new Vector2(2.0f, 2.0f);
            
            parameteres[i] = new Parameters(pos, life);
        }
        parameterBuffer.SetData(parameteres);
    }
    //---------------------------------

}
