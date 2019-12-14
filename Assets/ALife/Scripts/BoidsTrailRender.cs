using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsTrailRender : MonoBehaviour
{
    #region MeshData
    [SerializeField] int trailNum;
    [SerializeField] int trailLength;
    [SerializeField] float trailWidth;
    List<Vector3> verticesList;
    List<Vector3> normalList;
    List<Vector2> uvList;
    List<int> indexList;
    #endregion


    #region ComputeShader
    [SerializeField] ComputeShader cs;
    const int BLOCK_SIZE = 32;
    ComputeBuffer positionBuffer;
    ComputeBuffer velocityBuffer;
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


    #region BoidsParameter
    public float CohesionNeighborhoodRadius = 2.0f;
    public float AlignmentNeighborhoodRadius = 2.0f;
    public float SeparateNeighborhoodRadius = 1.0f;

    public float MaxSpeed;
    public float MaxSteerForce;

    public float CohesionWeight;
    public float AlignmentWeight;
    public float SeparateWeight;

    public float AvoidWallWeight;

    public Vector3 WallCenter = Vector3.zero;
    public Vector3 WallSize;
    #endregion

    void Start()
    {
        CreateMesh();
        initInstancingParameter();
        CreateBuffer();
        initBuffer();
        kernel = cs.FindKernel("update");
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            initBuffer();
            kernel = cs.FindKernel("update");
        }

        cs.SetBuffer(kernel, "positionBuffer", positionBuffer);
        cs.SetBuffer(kernel, "velocityBuffer", velocityBuffer);
        cs.SetFloat("dt", Time.deltaTime);
        cs.SetFloat("time", Time.realtimeSinceStartup);

        //send boids parameter
        
        cs.SetFloat("_CohesionNeighborhoodRadius", CohesionNeighborhoodRadius);
        cs.SetFloat("_AlignmentNeighborhoodRadius", AlignmentNeighborhoodRadius);
        cs.SetFloat("_SeparateNeighborhoodRadius", SeparateNeighborhoodRadius);
        cs.SetFloat("_MaxSpeed", MaxSpeed);
        cs.SetFloat("_MaxSteerForce", MaxSteerForce);
        cs.SetFloat("_SeparateWeight", SeparateWeight);
        cs.SetFloat("_CohesionWeight", CohesionWeight);
        cs.SetFloat("_AlignmentWeight", AlignmentWeight);
        cs.SetVector("_WallCenter", WallCenter);
        cs.SetVector("_WallSize", WallSize);
        cs.SetFloat("_AvoidWallWeight", AvoidWallWeight);
        cs.SetFloat("trailNum", trailNum);
        

        cs.Dispatch(kernel, trailNum, 1, 1);

        instancingMat.SetBuffer("positionBuffer", positionBuffer);
        instancingMat.SetBuffer("velocityBuffer", velocityBuffer);
        instancingMat.SetInt("BLOCK_SIZE", BLOCK_SIZE);
        Graphics.DrawMeshInstancedIndirect(srcMesh, 0, instancingMat,
        new Bounds(Vector3.zero, Vector3.one * 32.0f), argsBuffer);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(WallCenter, WallSize);
    }
    //--------------------------------------------------------------------------------------------------------
    void CreateMesh()
    {
        srcMesh = new Mesh();
        verticesList = new List<Vector3>();
        normalList = new List<Vector3>();
        uvList = new List<Vector2>();
        indexList = new List<int>();

        for(int i = 0; i < BLOCK_SIZE; i++)
        {
            float delta = trailLength / (float)BLOCK_SIZE;
            //var x = (i - BLOCK_SIZE / 2.0f) * delta;
            var z = i* delta;
            //Vector3 p1 = new Vector3(x, 0.0f, trailWidth / 2.0f);
            //Vector3 p2 = new Vector3(x, 0.0f, -1.0f * trailWidth / 2.0f);
            Vector3 p1 = new Vector3(trailWidth / 2.0f, 0.0f, z);
            Vector3 p2 = new Vector3(-1.0f * trailWidth / 2.0f, 0.0f, z);


            verticesList.Add(p1);
            verticesList.Add(p2);

            normalList.Add(p1.normalized);
            normalList.Add(p2.normalized);


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
        }

        srcMesh.vertices = verticesList.ToArray();
        srcMesh.normals = normalList.ToArray();
        srcMesh.uv = uvList.ToArray();

        //srcMesh.SetTriangles(indexList.ToArray(), 0);
        srcMesh.SetIndices(indexList.ToArray(), MeshTopology.Triangles, 0);
        srcMesh.RecalculateNormals();

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
        positionBuffer = new ComputeBuffer(trailNum * BLOCK_SIZE, sizeof(float) * 3);
        velocityBuffer = new ComputeBuffer(trailNum * BLOCK_SIZE, sizeof(float) * 3);
    }

    //--------------------------------------------------------------------------------------------------------
    private void initBuffer()
    {
        kernel = cs.FindKernel("init");
        cs.SetBuffer(kernel, "positionBuffer", positionBuffer);
        cs.SetBuffer(kernel, "velocityBuffer", velocityBuffer);
        cs.Dispatch(kernel, trailNum, 1, 1);
    }
    //--------------------------------------------------------------------------------------------------------
    private void OnDisable()
    {
        if(argsBuffer != null) argsBuffer.Release();
        if(positionBuffer != null) positionBuffer.Release();
        if(velocityBuffer != null) velocityBuffer.Release();
    }
}
