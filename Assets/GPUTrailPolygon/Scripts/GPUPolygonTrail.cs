using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUPolygonTrail : MonoBehaviour
{

    #region trail info
    [SerializeField] int trailNum;//dispatchの次元数と一致する
    [SerializeField] int trailPointNum;//何ポイントでトレイルを構成するか
    [SerializeField] int circleResolution; //断面の解像度
    [SerializeField] float radius;//断面の半径
    [SerializeField] float trailLength;
    List<Vector3> positionList;
    List<Vector3> normalList;
    List<Vector2> uvList;
    List<int> indexList;
    #endregion

    #region ComputeShaderParameters
    [SerializeField] ComputeShader cs;
    ComputeBuffer positionBuffer;
    ComputeBuffer velocityBuffer;
    ComputeBuffer pulseBuffer;
    int kernel;
    uint threadX, threadY, threadZ;
    #endregion

    #region instancing params
    ComputeBuffer argsBuffer;
    private uint[] args = new uint[5];
    private int instancingCount;
    [SerializeField] Mesh srcMesh;
    [SerializeField] Material instancingMat;
    #endregion


    [SerializeField] GameObject target;
    int pulse = 0;
    public float blend = 0.0f;//blend * noise + (1.0 - blend) * taraget

    private void Awake()
    {

    }

    void Start()
    {
        if(srcMesh == null)
        {
            CreateMesh();
        }
        InitInstancingParameter();
        CreateBuffers();
        InitBuffers();
        kernel = cs.FindKernel("update");

    }

    private void OnEnable()
    {
        
        if(srcMesh == null)
        {
            CreateMesh();
        }
        InitInstancingParameter();
        CreateBuffers();
        InitBuffers();
        kernel = cs.FindKernel("update");
        
    }

    void Update()
    {
        /*
        if(osc.oscData.Rythm == 1.0)
        {
            //target.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, Random.Range(-3.0f, 3.0f));
        }
        */


       //if(Input.GetKeyDown(KeyCode.A)){
       if(OscData.kick == 1) { 
            pulse = 1;
        }

        cs.SetFloat("time", Time.realtimeSinceStartup);
        //cs.SetInt("BLOCK_SIZE", trailPointNum);
        cs.SetInt("pulse", pulse);
        cs.SetFloat("blend", blend);
        cs.SetBuffer(kernel, "positionBuffer", positionBuffer);
        cs.SetBuffer(kernel, "velocityBuffer", velocityBuffer);
        cs.SetBuffer(kernel, "pulseBuffer", pulseBuffer);
        cs.SetVector("target", target.transform.position);
        cs.Dispatch(kernel, trailNum, 1, 1);


        instancingMat.SetFloat("trailNum", trailNum);
        instancingMat.SetInt("pulse", pulse);
        instancingMat.SetBuffer("positionBuffer", positionBuffer);
        instancingMat.SetBuffer("pulseBuffer", pulseBuffer);
        instancingMat.SetInt("BLOCK_SIZE", trailPointNum);
        instancingMat.SetInt("circleResolution", circleResolution);

        Graphics.DrawMeshInstancedIndirect(srcMesh, 0, instancingMat,
        new Bounds(Vector3.zero, Vector3.one * 32.0f), argsBuffer);

        pulse = 0;
    }


    //instancing用のmeshを生成
    //--------------------------------------------------------------------
    void CreateMesh()
    {
        srcMesh = new Mesh();
        positionList = new List<Vector3>();
        normalList = new List<Vector3>();
        uvList = new List<Vector2>();
        indexList = new List<int>();

        float theta = 2.0f * Mathf.PI / circleResolution;
        float delta = trailLength / trailPointNum;
        float deltaUV = 1.0f / (circleResolution-1);
        for(int i = 0; i < trailPointNum; i++)
        {
            radius = Random.Range(radius * 0.5f, radius);
            for(int j = 0; j < circleResolution; j++)
            {

                
                var p = new Vector3(radius * Mathf.Cos(theta * j),
                                    radius * Mathf.Sin(theta * j),
                                    i * delta);
                positionList.Add(p);
                normalList.Add(p);
                uvList.Add(new Vector2(1.0f/trailPointNum*i , deltaUV * j));
                if(i > 0)
                {
                    int i0 = j + circleResolution * i;
                    //int next = i0 + (j + 1) % circleResolution;
                    int i1 = (j+1) % circleResolution + circleResolution * i;
                    int i2 = j + circleResolution * (i - 1);
                    int i3 = (j+1) % circleResolution + circleResolution * (i - 1);

                    /*
                    indexList.Add(i0);
                    indexList.Add(i1);
                    indexList.Add(i2);

                    indexList.Add(i1);
                    indexList.Add(i3);
                    indexList.Add(i2);
                    */

                    indexList.Add(i0);
                    indexList.Add(i2);
                    indexList.Add(i1);

                    indexList.Add(i1);
                    indexList.Add(i2);
                    indexList.Add(i3);

                }
            }
        }

        //srcMesh.vertices = positionList.ToArray();
        srcMesh.SetVertices(positionList);
        srcMesh.normals = normalList.ToArray();
        srcMesh.uv = uvList.ToArray();
        //srcMesh.RecalculateNormals();
        //srcMesh.RecalculateBounds();
        //srcMesh.triangles = indexList.ToArray();
        //srcMesh.SetIndices(indexList.ToArray(), MeshTopology.Triangles, 0);
        srcMesh.SetIndices(indexList.ToArray(), MeshTopology.LineStrip, 0);
    }

    //--------------------------------------------------------------------------------------------------------
    private void InitInstancingParameter()
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
    //ComputeBuffer
    //--------------------------------------------------------------------
    void CreateBuffers()
    {
        positionBuffer = new ComputeBuffer(trailNum * trailPointNum, sizeof(float) * 3);
        velocityBuffer = new ComputeBuffer(trailNum * trailPointNum, sizeof(float) * 3);
        pulseBuffer = new ComputeBuffer(trailNum * trailPointNum, sizeof(float) * 2);
    }
    //--------------------------------------------------------------------
    void InitBuffers()
    {
        kernel = cs.FindKernel("init");
        cs.SetBuffer(kernel, "positionBuffer", positionBuffer);
        cs.SetBuffer(kernel, "velocityBuffer", velocityBuffer);
        cs.Dispatch(kernel, trailNum, 1, 1);

    }
    private void OnDisable()
    {
        positionBuffer.Release();
        velocityBuffer.Release();
        pulseBuffer.Release();
    }
}
