﻿using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using System.Runtime.InteropServices;

struct Particle
{
    public int id;
    public bool active;
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 rotation;
    public Vector3 angVelocity;
    public float scale;
    public float time;
    public float lifeTime;
}

public class PseudoInstancedGPUParticleManager : MonoBehaviour
{
    const int MAX_VERTEX_NUM = 65534;

    [SerializeField, Tooltip("This cannot be changed while running.")]
    int maxParticleNum;
    [SerializeField]
    Mesh mesh;
    [SerializeField]
    Shader shader;
    [SerializeField]
    ComputeShader computeShader;

    [SerializeField]
    Vector3 velocity = new Vector3(2f, 5f, 2f);
    [SerializeField]
    Vector3 angVelocity = new Vector3(45f, 45f, 45f);
    [SerializeField]
    Vector3 range = Vector3.one;

    [SerializeField] Mesh combinedMesh_;
    ComputeBuffer computeBuffer_;
    int updateKernel_;
    int emitKernel_;
    List<Material> materials_ = new List<Material>();
    int particleNumPerMesh_;
    int meshNum_;

    Mesh CreateCombinedMesh(Mesh mesh, int num)
    {
        Assert.IsTrue(mesh.vertexCount * num <= MAX_VERTEX_NUM);

        var meshIndices = mesh.GetIndices(0);
        var indexNum = meshIndices.Length;

        var vertices = new List<Vector3>();
        var indices = new int[num * indexNum];
        var normals = new List<Vector3>();
        var tangents = new List<Vector4>();
        var uv0 = new List<Vector2>();
        var uv1 = new List<Vector2>();

        for(int id = 0; id < num; ++id)
        {
            vertices.AddRange(mesh.vertices);
            normals.AddRange(mesh.normals);
            tangents.AddRange(mesh.tangents);
            uv0.AddRange(mesh.uv);

            // 各メッシュのインデックスは（1 つのモデルの頂点数 * ID）分ずらす
            for(int n = 0; n < indexNum; ++n)
            {
                indices[id * indexNum + n] = id * mesh.vertexCount + meshIndices[n];
            }

            // 2 番目の UV に ID を格納しておく
            for(int n = 0; n < mesh.uv.Length; ++n)
            {
                uv1.Add(new Vector2(id, id));
            }
        }

        var combinedMesh = new Mesh();
        combinedMesh.SetVertices(vertices);
        combinedMesh.SetIndices(indices, MeshTopology.Triangles, 0);
        combinedMesh.SetNormals(normals);
        combinedMesh.RecalculateNormals();
        combinedMesh.SetTangents(tangents);
        combinedMesh.SetUVs(0, uv0);
        combinedMesh.SetUVs(1, uv1);
        combinedMesh.RecalculateBounds();
        combinedMesh.bounds.SetMinMax(Vector3.one * -100f, Vector3.one * 100f);

        return combinedMesh;
    }

    void OnEnable()
    {
        particleNumPerMesh_ = MAX_VERTEX_NUM / mesh.vertexCount;
        meshNum_ = (int)Mathf.Ceil((float)maxParticleNum / particleNumPerMesh_);

        for(int i = 0; i < meshNum_; ++i)
        {
            var material = new Material(shader);
            material.SetInt("_IdOffset", particleNumPerMesh_ * i);
            materials_.Add(material);
        }

        combinedMesh_ = CreateCombinedMesh(mesh, particleNumPerMesh_);
        computeBuffer_ = new ComputeBuffer(maxParticleNum, Marshal.SizeOf(typeof(Particle)), ComputeBufferType.Default);

        var initKernel = computeShader.FindKernel("Init");
        updateKernel_ = computeShader.FindKernel("Update");
        emitKernel_ = computeShader.FindKernel("Emit");

        computeShader.SetBuffer(initKernel, "_Particles", computeBuffer_);
        computeShader.SetVector("_Velocity", velocity);
        computeShader.SetVector("_AngVelocity", angVelocity * Mathf.Deg2Rad);
        computeShader.SetVector("_Range", range);
        computeShader.Dispatch(initKernel, maxParticleNum / 8, 1, 1);
    }

    void OnDisable()
    {
        computeBuffer_.Release();
    }

    void Update()
    {
        computeShader.SetVector("_Velocity", velocity);
        computeShader.SetVector("_AngVelocity", angVelocity * Mathf.Deg2Rad);
        computeShader.SetVector("_Range", range);

        computeShader.SetBuffer(emitKernel_, "_Particles", computeBuffer_);
        computeShader.Dispatch(emitKernel_, maxParticleNum / 8, 1, 1);

        computeShader.SetFloat("_DeltaTime", Time.deltaTime);
        computeShader.SetBuffer(updateKernel_, "_Particles", computeBuffer_);
        computeShader.Dispatch(updateKernel_, maxParticleNum / 8, 1, 1);

        for(int i = 0; i < meshNum_; ++i)
        {
            var material = materials_[i];
            material.SetInt("_IdOffset", particleNumPerMesh_ * i);
            material.SetBuffer("_Particles", computeBuffer_);
            Graphics.DrawMesh(combinedMesh_, transform.position, transform.rotation, material, 0);
        }
    }
}
