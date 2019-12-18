using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DoomScene
{
    public class OSCManager : MonoBehaviour
    {

        public SceneManager sceneManager;
        #region munipulate data1
        public GameObject wireFrameSphere;
        private Material wireFrameMaterial;
        public GameObject rythmSphere;
        private Material extrudeMaterial;
        #endregion

        #region munipulate data2
        public GameObject NoiseBlock;
        public Material NoiseBlockMaterial;
        private AudioReactiveDoTween ardt;
        public Material UpdateMesh;
        #endregion

        private void Awake()
        {
            //FirstSection
            extrudeMaterial = rythmSphere.GetComponent<MeshRenderer>().material;
            wireFrameMaterial = wireFrameSphere.GetComponent<MeshRenderer>().material;
            //SecondSection
            ardt = NoiseBlock.GetComponent<AudioReactiveDoTween>();
        }
        void Start()
        {

        }

        void Update()
        {
            if(sceneManager.curSectionID == 1)
            {
                wireFrameMaterial.SetFloat("_amp", OscData.low);
                //transparent.SetFloat("_randomScale", oscData.low );
                //---------------------------------------------
                extrudeMaterial.SetFloat("_extrude", OscData.low);
                var snare = OscData.snare + 0.2f;
                extrudeMaterial.SetVector("_color", new Vector4(snare, snare, snare, 1.0f));
            } 
            else if(sceneManager.curSectionID == 2)
            {
                NoiseBlockMaterial.SetFloat("_AudioSeed", OscData.low);
                if(OscData.snare == 1)
                {
                    NoiseBlockMaterial.SetFloat("_Snare", UnityEngine.Random.Range(1.0f, 10.0f));
                    UpdateMesh.SetFloat("_Audio", UnityEngine.Random.Range(1.0f, 10.0f));
                }

                ardt.KickDetection = 0;
                if(OscData.kick == 1)
                {
                    ardt.KickDetection = 1;
                }
            }
        }
    }
}