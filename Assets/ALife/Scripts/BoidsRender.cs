using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoidsSimulationOnGPU
{
    [RequireComponent(typeof(GPUBoids))]
    public class BoidsRender : MonoBehaviour
    {
        #region Paremeters
        public Vector3 ObjectScale = new Vector3(0.1f, 0.2f, 0.5f);
        #endregion

        #region Script References
        public GPUBoids GPUBoidsScript;
        #endregion

        #region Built-in Resources
        public Mesh InstanceMesh;
        public Material InstanceRenderMaterial;
        #endregion

        #region Private Variables
        uint[] args = new uint[5] { 0, 0, 0, 0, 0 };
        ComputeBuffer argsBuffer;
        #endregion

        #region MonoBehaviour Functions
        void Start()
        {
            argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint),
                ComputeBufferType.IndirectArguments);
        }

        void Update()
        {
            RenderInstancedMesh();
        }

        void OnDisable()
        {
            if(argsBuffer != null)
                argsBuffer.Release();
            argsBuffer = null;
        }
        #endregion

        #region Private Functions
        void RenderInstancedMesh()
        {
            if(InstanceRenderMaterial == null || GPUBoidsScript == null ||
                !SystemInfo.supportsInstancing)
                return;

            uint numIndices = (InstanceMesh != null) ?
                (uint)InstanceMesh.GetIndexCount(0) : 0;
            args[0] = numIndices; // メッシュのインデックス数をセット
            args[1] = (uint)GPUBoidsScript.GetMaxObjectNum(); // インスタンス数をセット
            argsBuffer.SetData(args); // バッファにセット

            InstanceRenderMaterial.SetBuffer("_BoidDataBuffer",
                GPUBoidsScript.GetBoidDataBuffer());
            InstanceRenderMaterial.SetVector("_ObjectScale", ObjectScale);
            var bounds = new Bounds
            (
                GPUBoidsScript.GetSimulationAreaCenter(), 
                GPUBoidsScript.GetSimulationAreaSize()    
            );
           
            Graphics.DrawMeshInstancedIndirect
            (
                InstanceMesh,           
                0,                      
                InstanceRenderMaterial,
                bounds,                 
                argsBuffer              
            );
        }
        #endregion
    }
}