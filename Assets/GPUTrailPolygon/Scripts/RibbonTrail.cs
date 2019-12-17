using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RibbonTrail : MonoBehaviour
{
    #region MeshData
    [SerializeField] int trailNum;
    [SerializeField] int trailResolution; //GROUP_BLOCK_SIZE
    [SerializeField] float trailLength; //PhysicalLength
    [SerializeField] float trailWidth; //PhysicalWidth
    List<Vector3> verticesList;
    List<Vector3> normalList;
    List<Vector2> uvList;
    List<int> indexList;
    #endregion


    #region ComputeShader
    [SerializeField] float dx;
    [SerializeField] float dz;
    public float amp;
    [SerializeField] ComputeShader cs;
    const int BLOCK_SIZE = 128;
    ComputeBuffer positionBuffer;
    ComputeBuffer velocityBuffer;
    ComputeBuffer normalBuffer;
    ComputeBuffer topPosBuffer;
    int kernel;
    int threadGroupSize;
    #endregion

    #region instancingParams
    ComputeBuffer argsBuffer;
    private uint[] args = new uint[5];
    private int instancingCount;
    [SerializeField] Mesh srcMesh;
    [SerializeField] Material instancingMat;
    #endregion



    void Start()
    {
       if(trailResolution != BLOCK_SIZE)
        {
            Debug.Log("miss match");
        }

        CreateandSetMesh();
        initInstancingParameter();
        CreateBuffer();
        UpdateBuffers();
    }

    void Update()
    {
        kernel = cs.FindKernel("update");
        cs.SetFloat("low", OscData.low);
        cs.SetFloat("kick", OscData.kick);
        cs.SetFloat("rythm", OscData.rythm);
        cs.SetFloat("amp", amp);


        cs.SetBuffer(kernel, "positionBuffer", positionBuffer);
        cs.SetBuffer(kernel, "velocityBuffer", velocityBuffer);
        cs.SetBuffer(kernel, "normalBuffer", normalBuffer);
        cs.SetBuffer(kernel, "topPosBuffer", topPosBuffer);
        cs.SetFloat("time", Time.realtimeSinceStartup);
        cs.Dispatch(kernel, trailNum, 1, 1);




        instancingMat.SetFloat("trailNum", trailNum);
        instancingMat.SetFloat("dx", dx);
        instancingMat.SetFloat("dz", dz);
        instancingMat.SetBuffer("positionBuffer", positionBuffer);
        instancingMat.SetBuffer("velocityBuffer", velocityBuffer);
        instancingMat.SetBuffer("normalBuffer", normalBuffer);
        instancingMat.SetInt("BLOCK_SIZE", BLOCK_SIZE);
        Graphics.DrawMeshInstancedIndirect(srcMesh, 0, instancingMat,
        new Bounds(Vector3.zero, Vector3.one * 32.0f), argsBuffer);
    }
    //--------------------------------------------------------------------------------------------------------
    void CreateandSetMesh()
    {
        srcMesh = new Mesh();
        verticesList = new List<Vector3>();
        normalList = new List<Vector3>();
        indexList = new List<int>();

        for(int i = 0; i < BLOCK_SIZE; i++)
        {
            
            float delta = trailLength / (float)trailResolution;
            var z = -i * delta;
            /*
            Vector3 p1 = new Vector3(trailWidth / 2.0f, 0.0f, z);
            Vector3 p2 = new Vector3(-1.0f * trailWidth / 2.0f, 0.0f, z);
            verticesList.Add(p1);
            verticesList.Add(p2);
            normalList.Add(new Vector3(0, 1, 0));
            normalList.Add(new Vector3(0, 1, 0));
            uvList.Add(new Vector2((float)i / (float)(BLOCK_SIZE - 1.0), 0.0f));
            uvList.Add(new Vector2((float)i / (float)(BLOCK_SIZE - 1.0), 1.0f));
            if(i > 1 && i % 2 == 0)
            {
                int i0 = i;
                int i1 = i + 1;
                int i2 = i - 1;
                int i3 = i - 2;
                indexList.Add(i0);
                indexList.Add(i1);
                indexList.Add(i2);

                indexList.Add(i0);
                indexList.Add(i2);
                indexList.Add(i3);
            }
            */
            Vector3 p1 = new Vector3(trailWidth / 2.0f, 0.0f, z);
            verticesList.Add(p1);
            normalList.Add(new Vector3(0, 1, 0));
            indexList.Add(i);
        }

        srcMesh.vertices = verticesList.ToArray();
        srcMesh.normals = normalList.ToArray();
        srcMesh.SetIndices(indexList.ToArray(), MeshTopology.LineStrip, 0);
    }
    //--------------------------------------------------------------------------------------------------------
    private void initInstancingParameter()
    {
        instancingCount = trailNum;
        argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);

        args[0] = srcMesh.GetIndexCount(0);
        args[1] = (uint)instancingCount;
        args[2] = srcMesh.GetIndexStart(0);
        args[3] = srcMesh.GetBaseVertex(0);
        args[4] = 0;
        argsBuffer.SetData(args);
    }
    //--------------------------------------------------------------------------------------------------------
    private void CreateBuffer()
    {
        //BLOCK_SIZE : 1本のTrailの解像度に相当
        positionBuffer = new ComputeBuffer(trailNum * trailResolution, sizeof(float) * 3);
        velocityBuffer = new ComputeBuffer(trailNum * trailResolution, sizeof(float) * 3);
        normalBuffer = new ComputeBuffer(trailNum * trailResolution, sizeof(float) * 3);
        topPosBuffer = new ComputeBuffer(trailNum, sizeof(float) * 3);

    }
    //--------------------------------------------------------------------------------------------------------
    private void UpdateBuffers()
    {
        int vertexNum = trailNum * trailResolution;
        Vector3[] positionArr = new Vector3[vertexNum];
        Vector3[] velocityArr = new Vector3[vertexNum];
        Vector3[] normalArr = new Vector3[vertexNum];
        Vector3[] topPosArr = new Vector3[trailNum];
    

        for(int i = 0; i < vertexNum; i++)
        {
            positionArr[i] = Vector3.zero;
            velocityArr[i] = Vector3.zero;
            normalArr[i] = new Vector3(0, 1, 0);
        }

        for(int i = 0; i < trailNum; i++)
        {
            topPosArr[i] = Random.insideUnitSphere;
        }

        positionBuffer.SetData(positionArr);
        velocityBuffer.SetData(velocityArr);
        normalBuffer.SetData(normalArr);
        topPosBuffer.SetData(topPosArr);
    }
    //--------------------------------------------------------------------------------------------------------
    private void OnDisable()
    {
        if(argsBuffer != null) argsBuffer.Release();
        if(positionBuffer != null) positionBuffer.Release();
        if(velocityBuffer != null) velocityBuffer.Release();
        if(normalBuffer != null) normalBuffer.Release();
        if(topPosBuffer != null) topPosBuffer.Release();
    }

}
