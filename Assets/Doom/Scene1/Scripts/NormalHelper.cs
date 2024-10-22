﻿using UnityEngine;

[ExecuteInEditMode]
public class NormalHelper : MonoBehaviour
{

    public float length = 1;

    public Vector3 bias;

    void Update()
    {


        var meshFilt = GetComponent<MeshFilter>();
        if (meshFilt == null) return;

        var mesh = meshFilt.mesh;

        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;

        for (var i = 0; i < normals.Length; i++)
        {
            Vector3 pos = vertices[i];
            pos.x *= transform.localScale.x;
            pos.y *= transform.localScale.y;
            pos.z *= transform.localScale.z;
            pos += transform.position + bias;

            Debug.DrawLine
            (
                pos,
                pos + normals[i] * length, Color.red);
        }
    }
}
