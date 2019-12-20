using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class GPUBoids : MonoBehaviour
{
    [System.Serializable]
    struct BoidData
    {
        public Vector3 Velocity; 
        public Vector3 Position; 
    }

    const int SIMULATION_BLOCK_SIZE = 256;

    #region Boids Parameters
    [Range(256, 32768)]
    public int MaxObjectNum = 16384;
    public float CohesionNeighborhoodRadius = 2.0f;
    public float AlignmentNeighborhoodRadius = 2.0f;
    public float SeparateNeighborhoodRadius = 1.0f;

    public float MaxSpeed = 5.0f;
    public float MaxSteerForce = 0.5f;

    public float CohesionWeight = 1.0f;
    public float AlignmentWeight = 1.0f;
    public float SeparateWeight = 3.0f;
    public float AvoidWallWeight = 10.0f;

    public Vector3 WallCenter = Vector3.zero;
    public Vector3 WallSize = new Vector3(32.0f, 32.0f, 32.0f);
    #endregion

    #region Built-in Resources
    public ComputeShader BoidsCS;
    public Vector4 normalColor, activeColor = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
    public float low;
    public float kick;

    int mode = 0;
    public bool isAudioReactive = false;
    public Material boidsRender;
    #endregion

    #region Private Resources
    ComputeBuffer _boidForceBuffer;
    ComputeBuffer _boidDataBuffer;
    #endregion


    #region Accessors
    public ComputeBuffer GetBoidDataBuffer()
    {
        return this._boidDataBuffer != null ? this._boidDataBuffer : null;
    }

    public int GetMaxObjectNum()
    {
        return this.MaxObjectNum;
    }

    public Vector3 GetSimulationAreaCenter()
    {
        return this.WallCenter;
    }

    public Vector3 GetSimulationAreaSize()
    {
        return this.WallSize;
    }
    #endregion


    
    public float maxSteerForceonKick;
    public float maxSpeedonKick;
    public float separateWeightonKick;



    void Start()
    {
        InitBuffer();
        //oscServer = osc.GetComponent<OSCManager>();
    }

    void Update()
    {
        Simulation();


        if(isAudioReactive)
        {
            //low
            if (OscData.kick > 0f)
            {
                //Debug.Log("kick");
                /*
                MaxSteerForce = 200.0f;
                MaxSpeed = 100.0f;
                SeparateWeight = 15f;
                */
                MaxSteerForce = maxSteerForceonKick;
                MaxSpeed = maxSpeedonKick;
                SeparateWeight = separateWeightonKick;
                boidsRender.SetColor("_Color", activeColor);
            }
            else
            {
                SeparateWeight = 0.2f;
                MaxSteerForce = 0.75f;
                MaxSpeed = 10.0f;
                boidsRender.SetColor("_Color", normalColor);
            }


            //separate
            //SeparateWeight = oscServer.separate;
        }
    }

    #region private function
    void InitBuffer()
    {
        //init buffer
        _boidDataBuffer = new ComputeBuffer(MaxObjectNum, Marshal.SizeOf(typeof(BoidData)));
        _boidForceBuffer = new ComputeBuffer(MaxObjectNum, Marshal.SizeOf(typeof(Vector3)));

        var forceArr = new Vector3[MaxObjectNum];
        var boidDataArr = new BoidData[MaxObjectNum];
        for(int i = 0; i < MaxObjectNum; i++)
        {
            forceArr[i] = Vector3.zero;
            boidDataArr[i].Position = Random.insideUnitSphere * 1.0f;
            boidDataArr[i].Velocity = Random.insideUnitSphere * 0.1f;
        }

        _boidForceBuffer.SetData(forceArr);
        _boidDataBuffer.SetData(boidDataArr);
        forceArr = null;
        boidDataArr = null;

    }

    void Simulation()
    {
        ComputeShader cs = BoidsCS;
        int id = -1;

        int threadGroupSize = Mathf.CeilToInt(MaxObjectNum / SIMULATION_BLOCK_SIZE);

        
            // 操舵力を計算
        id = cs.FindKernel("ForceCS"); // カーネルIDを取得
        cs.SetInt("_MaxBoidObjectNum", MaxObjectNum);
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
        cs.SetBuffer(id, "_BoidDataBufferRead", _boidDataBuffer);
        cs.SetBuffer(id, "_BoidForceBufferWrite", _boidForceBuffer);
        cs.Dispatch(id, threadGroupSize, 1, 1); // ComputeShaderを実行

        // 操舵力から、速度と位置を計算
        id = cs.FindKernel("IntegrateCS"); // カーネルIDを取得
        cs.SetFloat("_DeltaTime", Time.deltaTime);
        cs.SetBuffer(id, "_BoidForceBufferRead", _boidForceBuffer);
        cs.SetBuffer(id, "_BoidDataBufferWrite", _boidDataBuffer);
        cs.Dispatch(id, threadGroupSize, 1, 1); // ComputeShaderを実行
         
    }

    private void OnDisable()
    {
        ReleaseBuffer();
    }


    private void OnDestroy()
    {
        ReleaseBuffer();
    }
    void ReleaseBuffer()
    {
        if (_boidDataBuffer != null)
        {
            _boidDataBuffer.Release();
            _boidDataBuffer = null;
        }

        if (_boidForceBuffer != null)
        {
            _boidForceBuffer.Release();
            _boidForceBuffer = null;
        }
    }
    #endregion
}
