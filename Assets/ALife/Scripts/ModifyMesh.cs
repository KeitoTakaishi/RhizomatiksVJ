using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyMesh : MonoBehaviour
{
    enum TopologyType
    {
        lines,
        triangles,
    }
    #region mesh data 
    Mesh mesh;
    int[] bufferIndexArray;
    List<int> index;
    uint indexCount;
    uint curIndexCount = 0;
    [SerializeField]
    TopologyType topologyType;
    #endregion

    #region public data
    public Vector3[] points = new Vector3[3];
    public bool scanDone = false;
    #endregion

    void Start()
    {

        topologyType = new TopologyType();
        topologyType = TopologyType.lines;
        mesh = this.GetComponent<MeshFilter>().mesh;
        index = new List<int>();
        indexCount = mesh.GetIndexCount(0);
        bufferIndexArray = new int [indexCount];
        bufferIndexArray = mesh.GetIndices(0);
        mesh.GetIndices(0).CopyTo(bufferIndexArray, 0);

        if(topologyType == TopologyType.lines)
        {
            mesh.SetIndices(index.ToArray(), MeshTopology.LineStrip, 0);
        } else if(topologyType == TopologyType.triangles)
        {
            mesh.SetIndices(index.ToArray(), MeshTopology.Triangles, 0);
        }
    }
    
    float modelScale = 10.0f;
    void Update()
    {
        scanVertex();
        setTopology(scanDone);
    }

    void scanVertex()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            RemoveIndex();
        }

        if(curIndexCount + 3 < indexCount)
        {
            for(int i = (int)curIndexCount; i < curIndexCount + 3; i++)
            {
                index.Add(bufferIndexArray[i]);
                var v = mesh.vertices[index[i]] * modelScale;
                points[i % 3] = new Vector3(-1 * v.x, v.y, v.z);
            }
        } else
        {
            scanDone = true;
        }
        curIndexCount += 3;
    }

    void setTopology(bool scanDone)
    {
        if(scanDone)
        {
            mesh.SetIndices(index.ToArray(), MeshTopology.Triangles, 0);
        } else
        {
            mesh.SetIndices(index.ToArray(), MeshTopology.LineStrip, 0);
        }
    }

    public void RemoveIndex()
    {
        index.Clear();
        curIndexCount = 0;
        scanDone = false;

    } 
}
