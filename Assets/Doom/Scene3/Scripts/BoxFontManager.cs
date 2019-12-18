using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxFontManager : MonoBehaviour
{
    [SerializeField] private GameObject TrasparentBox;
    [SerializeField] private GameObject TrasparentBox2;
    [SerializeField] private int num;
    [SerializeField] private GameObject thirdStage;
    [SerializeField] Vector2 emitRange;
    [SerializeField] int LineSpan = 10;

    private GameObject[] boxType1;
    private GameObject[] boxType2;
    private Vector3[] initPos1;
    private Vector3[] initPos2;
    private float offSetY = 5000;
    private MeshFilter mf;

    public int kick;
    

    private void OnEnable()
    {
        boxType1 = new GameObject[num];
        boxType2 = new GameObject[num];
        initPos1 = new Vector3[num];
        initPos2 = new Vector3[num];

        for(int i = 0; i < num; i++)
        {
            boxType1[i] = (GameObject)Instantiate(TrasparentBox);
            boxType2[i] = (GameObject)Instantiate(TrasparentBox2);
            initPos1[i] = new Vector3(UnityEngine.Random.Range(-emitRange.x, emitRange.x), UnityEngine.Random.Range(-30.0f, 5.0f) + offSetY, UnityEngine.Random.Range(-emitRange.y, emitRange.y));
            initPos2[i] = new Vector3(UnityEngine.Random.Range(-emitRange.x, emitRange.x), UnityEngine.Random.Range(-30.0f, 5.0f) + offSetY, UnityEngine.Random.Range(-emitRange.y, emitRange.y));
            boxType1[i].transform.parent = thirdStage.transform;
            boxType2[i].transform.parent = thirdStage.transform;

            var scale = UnityEngine.Random.Range(1.0f, 3.0f);
            boxType1[i].transform.localScale = new Vector3(scale, scale, scale);
            boxType2[i].transform.localScale = new Vector3(scale, scale, scale);
            boxType1[i].transform.position = initPos1[i];
            boxType2[i].transform.position = initPos2[i];
        }
        mf = this.GetComponent<MeshFilter>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
        
        for(int i = 0; i < num; i++)
        {

            if(boxType1[i].transform.position.y > 20.0f + offSetY )
            {
                boxType1[i].transform.position = initPos1[i] + new Vector3(0, 0, 0);
                boxType2[i].transform.position = initPos2[i] + new Vector3(0, 0, 0);
            }

            boxType1[i].transform.position += new Vector3(0, UnityEngine.Random.Range(0.05f, 0.1f), 0); 
            boxType2[i].transform.position += new Vector3(0, UnityEngine.Random.Range(0.05f, 0.1f), 0);
        }
        
        
        if(Time.frameCount % LineSpan == 0)
        {
            var indices = new List<int>();
            var vertices = new List<Vector3>();
            var mesh = new Mesh();
            for(int i = 0; i < 30; i++)
            {
                var i1 = UnityEngine.Random.Range(0, num);
                var a = boxType1[i1].transform.localPosition;
                indices.Add(i);
                vertices.Add(a);
                var i2 = UnityEngine.Random.Range(0, num);
                var b = boxType1[i2].transform.localPosition;
                indices.Add(i + 1);
                vertices.Add(b);
            }
            mesh.SetVertices(vertices);
            mesh.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);
            mf.sharedMesh = mesh;
            LineSpan = UnityEngine.Random.Range(1, 10);
        }


        if(kick == 1)
        {
            for(int i = 0; i < 30; i++)
            {
                boxType1[i].GetComponent<MeshRenderer>().material.SetFloat("width", UnityEngine.Random.Range(1.0f, 30.0f));
                boxType2[i].GetComponent<MeshRenderer>().material.SetFloat("width", UnityEngine.Random.Range(1.0f, 30.0f));
            }
        }
    }


    private void OnDisable()
    {
        for(int i = 0; i < num; i++)
        {
            Destroy(boxType1[i]);
            Destroy(boxType2[i]);
        }
        mf = null;
    }
}
