using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoomScene
{
    public class SceneManager : MonoBehaviour
    {

        #region public data
        public int curSectionID = 1;
        #endregion

        #region firstSection data
        public GameObject firstSection;
        public GameObject wireFarmeSphere;
        public GameObject rythmSpherer;
        public GameObject upperLineTrail;
        public GameObject sphereShapeLineBurst;
        public GameObject boxParticle;
        public GameObject upperQuad;
        public GameObject doom;
        public GameObject secondSection;
        Material wireFrameMat;
        Material extrudeMat;
        int wireFrameShaderMode = 0;
        int rythmSphereColorMode = 0;
        #endregion

        #region secondSection
        public GameObject NoiseBox;
        public GameObject HeadMesh;
        public GameObject trailParticleSystem;
        public GameObject triangleMeshParticle;
        public GameObject prismParticle;
        public GameObject humanModelPivot;
        public GameObject humanModel;
        #endregion

        #region scene3 public data
        public GameObject thirdSection;
        //public GameObject trailParticleSyste03;
        //public GameObject textParticleSyste03;
        //public Material defaultSkyBox;
        //public Material[] skyBoxMats = new Material[2];
        //AudioReactiveText audioReactiveText;
        //int skyBoxMode = 0;
        //public GameObject polygonParticle;
        //public GameObject boxFontManager;
        #endregion

        private void Awake()
        {
            //Debug.Log("Awake");
        }

        private void OnEnable()
        {
            //Debug.Log("OnEnable");
        }

        void Start()
        {
            //Debug.Log("Start");
            if (wireFrameMat == null)
            {
                wireFrameMat = wireFarmeSphere.GetComponent<MeshRenderer>().material;
                extrudeMat = rythmSpherer.GetComponent<MeshRenderer>().material;
                curSectionID = 1;
            }
        }

        void Update()
        {
            if(curSectionID == 1)
            {
                if(Input.GetKeyDown(KeyCode.Q))
                {
                    wireFarmeSphere.SetActive(!wireFarmeSphere.activeSelf);

                } else if(Input.GetKeyDown(KeyCode.W))
                {
                    wireFrameShaderMode = (wireFrameShaderMode + 1) % 3;
                    wireFrameMat.SetInt("_mode", wireFrameShaderMode);
                } else if(Input.GetKeyDown(KeyCode.E))
                {
                    rythmSpherer.SetActive(!rythmSpherer.activeSelf);
                } else if(Input.GetKeyDown(KeyCode.R))
                {
                    rythmSphereColorMode = (rythmSphereColorMode + 1) % 2;
                    extrudeMat.SetInt("_colorMode", rythmSphereColorMode);
                } else if(Input.GetKeyDown(KeyCode.T))
                {
                    upperLineTrail.SetActive(!upperLineTrail.activeSelf);
                } else if(Input.GetKeyDown(KeyCode.Y))
                {
                    sphereShapeLineBurst.SetActive(!sphereShapeLineBurst.activeSelf);
                } else if(Input.GetKeyDown(KeyCode.U))
                {
                    boxParticle.SetActive(!boxParticle.activeSelf);
                } else if(Input.GetKeyDown(KeyCode.I))
                {
                    upperQuad.SetActive(!upperQuad.activeSelf);
                } else if(Input.GetKeyDown(KeyCode.P))
                {
                    curSectionID = 2;
                    firstSection.SetActive(false);
                    secondSection.SetActive(!secondSection.activeSelf);
                    NoiseBox.SetActive(true);

                }
            }
            else if(curSectionID == 2)
            {
                if(Input.GetKeyDown(KeyCode.Q))
                {
                    HeadMesh.SetActive(!HeadMesh.activeSelf);
                } else if(Input.GetKeyDown(KeyCode.W))
                {
                    triangleMeshParticle.SetActive(!triangleMeshParticle.activeSelf);

                } else if(Input.GetKeyDown(KeyCode.E))
                {
                    //prismParticle.SetActive(!prismParticle.activeSelf);

                } else if(Input.GetKeyDown(KeyCode.R))
                {
                    NoiseBox.SetActive(!NoiseBox.activeSelf);

                } else if(Input.GetKeyDown(KeyCode.T))
                {
                    humanModelPivot.SetActive(!humanModelPivot.activeSelf);
                } else if(Input.GetKeyDown(KeyCode.Y))
                {
                    trailParticleSystem.SetActive(!trailParticleSystem.activeSelf);
                }
                  //go to scene3 
                  else if(Input.GetKeyDown(KeyCode.L))
                {
                    thirdSection.SetActive(true);
                    curSectionID = 3;
                    secondSection.SetActive(false);
                    //prismParticle.SetActive(false);
                    triangleMeshParticle.SetActive(false);
                    //polygonParticle.SetActive(true);
                }

            }
        }
    }
}
