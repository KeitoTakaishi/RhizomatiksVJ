using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
[ExecuteInEditMode]
public class Doom : MonoBehaviour
{

    Vector3 vertices;
    List<int> triangles;
    [SerializeField] [Range(1, 192)] int _wSegments = 16 , _hSegment = 8;
    [SerializeField] [Range(3.0f, 500.0f)] float _radius = 5.0f, _height = 1.0f;
    const float PI2 = Mathf.PI * 2f;


    #region base
    MeshFilter _filter;
    MeshRenderer _meshRenderer;
    #endregion


    private void Awake()
    {
        _filter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();

    }

    void Start()
    {
        _filter.sharedMesh = CreateMesh();
        //_filter.mesh.SetIndices(_filter.sharedMesh.triangles, MeshTopology.Lines, 0);
        //SubMeshApply();

    }

    void Update()
    {
    }

    Mesh CreateMesh()
    {
        var _mesh = new Mesh();
        var _vertices = new List<Vector3>();
        var _uvs = new List<Vector2>();
        var _normals = new List<Vector2>();


        for (int i = 0; i < _hSegment; i++)
        {
            for (int j = 0; j < _wSegments; j++)
            {
                var _theta = (PI2 / _wSegments) * (float)j;
                var x = _radius * Mathf.Cos(_theta);
                var y = _height * (i - _hSegment/2.0f);
                var z = _radius * Mathf.Sin(_theta);
                _vertices.Add(new Vector3(x, y, z));
                _uvs.Add(new Vector3(x/_wSegments, y/_height, z/_wSegments));
            }
        }


        var triangles = new List<int>();
        for(int y = 0; y < _hSegment - 1; y++)
        {
            for(int x = 0; x < _wSegments; x++)
            {
                int index = y * _wSegments + x;
                var a = index;
                var b = index + 1;
                var c = index + _wSegments;
                var d = index + _wSegments + 1;

                if (x != _wSegments - 1)
                {
                    /*
                    triangles.Add(a);
                    triangles.Add(c);
                    triangles.Add(d);
                    triangles.Add(a);
                    triangles.Add(d);
                    triangles.Add(b);
                    */


                    //back
                    
                    triangles.Add(a);
                    triangles.Add(d);
                    triangles.Add(c);
                    triangles.Add(a);
                    triangles.Add(b);
                    triangles.Add(d);
                    
                    //Debug.Log("Tri1");
                }
                else if(x == _wSegments - 1)
                {
                    b = y * _wSegments;
                    d = b + _wSegments;

                    /*
                    triangles.Add(a);
                    triangles.Add(c);
                    triangles.Add(d);
                    triangles.Add(a);
                    triangles.Add(d);
                    triangles.Add(b);
                    */

                    
                    //back
                    
                    triangles.Add(a);
                    triangles.Add(d);
                    triangles.Add(c);
                    triangles.Add(a);
                    triangles.Add(b);
                    triangles.Add(d);
                    
                    //Debug.Log("Tri2");
                }
            }
        }


        _mesh.vertices = _vertices.ToArray();
        _mesh.triangles = triangles.ToArray();
        this.triangles = triangles;
        _mesh.uv = _uvs.ToArray();
        _mesh.RecalculateBounds();
        _mesh.RecalculateNormals();
        //_filter.mesh.SetIndices(triangles.ToArray(), MeshTopology.Lines, 0);
        return _mesh;
    }


    void SubMeshApply()
    {
        int[] triangleCount = new int[2] { 15000, 15000 };
        this._filter.sharedMesh.subMeshCount = 3;

       this._filter.sharedMesh.SetTriangles(this.triangles.GetRange(0, triangleCount[0]), 0);
       this._filter.sharedMesh.SetTriangles(this.triangles.GetRange(triangleCount[0], triangleCount[1]), 1);
       this._filter.sharedMesh.SetTriangles(this.triangles.GetRange(triangleCount[0] + triangleCount[1], this.triangles.Count - triangleCount[0] - triangleCount[1]), 2);


    }
}
