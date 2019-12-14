using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DrawIndirect_FragmentShader
{
    public class GPUParticleSystem : MonoBehaviour
    {
        public int instanceCount = 100000;
        public Mesh instanceMesh;
        public Material instanceMaterial;
        public int subMeshIndex = 0;

        private ComputeBuffer positionBuffer;
        private ComputeBuffer argsBuffer;
        private uint[] args = new uint[5] { 0, 0, 0, 0, 0 };

        void Start()
        {
            argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
            UpdateBuffers();
        }

        void Update()
        {
            Graphics.DrawMeshInstancedIndirect(instanceMesh, subMeshIndex, instanceMaterial,
                new Bounds(Vector3.zero, new Vector3(100.0f, 100.0f, 100.0f)), argsBuffer);
        }

        void UpdateBuffers()
        {
            if(positionBuffer != null) positionBuffer.Release();
            positionBuffer = new ComputeBuffer(instanceCount, sizeof(float) * 4);

            Vector4[] positions = new Vector4[instanceCount];
            for(int i = 0; i < instanceCount; i++)
            {
                float angle = Random.Range(0.0f, Mathf.PI * 2.0f);
                float distance = Random.Range(20.0f, 100.0f);
                float height = Random.Range(-2.0f, 2.0f);
                float size = Random.Range(1.0f, 0.5f);
                positions[i] = new Vector4(Mathf.Sin(angle) * distance, height, Mathf.Cos(angle) * distance, size);
            }

            positionBuffer.SetData(positions);
            instanceMaterial.SetBuffer("positionBuffer", positionBuffer);

            // Indirect args
            if(instanceMesh != null)
            {
                args[0] = (uint)instanceMesh.GetIndexCount(subMeshIndex);
                args[1] = (uint)instanceCount;
                args[2] = (uint)instanceMesh.GetIndexStart(subMeshIndex);
                args[3] = (uint)instanceMesh.GetBaseVertex(subMeshIndex);
            } else
            {
                args[0] = args[1] = args[2] = args[3] = 0;
            }
            argsBuffer.SetData(args);


        }

        void OnDisable()
        {
            if(positionBuffer != null)
                positionBuffer.Release();
            positionBuffer = null;

            if(argsBuffer != null)
                argsBuffer.Release();
            argsBuffer = null;
        }
    }
}
