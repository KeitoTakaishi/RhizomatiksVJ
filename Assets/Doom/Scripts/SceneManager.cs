using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoomScene
{
    public class SceneManager : MonoBehaviour
    {

        #region public data
        public int curSectionID = 1;
        public CameraAnimation camAnim;
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
        public GameObject trailParticleSyste03;
        //public GameObject textParticleSyste03;
        public Material defaultSkyBox;
        public Material[] skyBoxMats = new Material[2];
        //AudioReactiveText audioReactiveText;
        int skyBoxMode = 0;
        //public GameObject polygonParticle;
        //public GameObject boxFontManager;
        #endregion


        #region scene4 public data
        public GameObject fourthSection;
        public GameObject walkingModel;
        public GameObject gpuBoidsGameObject;
        GPUBoids gpuBoids;
        #endregion

        #region scene5 public data
        public GameObject fifthSection;
        public AttributeBaker ab;
        public GameObject fifthSectionCamTarget;
        public GameObject pentagonMeshpartcile;
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
            curSectionID = 1;
            camAnim.target = wireFarmeSphere.transform;
            if (wireFrameMat == null)
            {
                wireFrameMat = wireFarmeSphere.GetComponent<MeshRenderer>().material;
            }

            if(extrudeMat == null)
            {
                extrudeMat = rythmSpherer.GetComponent<MeshRenderer>().material;
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
                    //triangleMeshParticle.SetActive(false);
                    //polygonParticle.SetActive(true);
                    camAnim.target = trailParticleSyste03.transform;
                }

            } else if(curSectionID == 3)
            {
                if(Input.GetKeyDown(KeyCode.Q))
                {
                    //doom.SetActive(!doom.activeSelf);

                } else if(Input.GetKeyDown(KeyCode.W))
                {
                    skyBoxMode += 1;
                    RenderSettings.skybox = skyBoxMats[(skyBoxMode) % 2];
                } else if(Input.GetKeyDown(KeyCode.E))
                {
                    //boxFontManager.SetActive(!boxFontManager.activeSelf);
                } else if(Input.GetKeyDown(KeyCode.R))
                {
                    trailParticleSyste03.SetActive(!trailParticleSyste03.activeSelf);
                } else if(Input.GetKeyDown(KeyCode.T))
                {
                    //textParticleSyste03.SetActive(!textParticleSyste03.activeSelf);

                } else if(Input.GetKeyDown(KeyCode.Y))
                {
                    //polygonParticle.SetActive(!polygonParticle.activeSelf);

                } else if(Input.GetKeyDown(KeyCode.M))
                {
                    thirdSection.SetActive(false);
                    fourthSection.SetActive(true);
                    camAnim.target = walkingModel.transform;
                    RenderSettings.skybox = defaultSkyBox;
                    //polygonParticle.SetActive(false);
                    curSectionID = 4;
                }
            } else if(curSectionID == 4)
            {
                if(Input.GetKeyDown(KeyCode.Q))
                {
                    //gpuBoidsGameObject.SetActive(!gpuBoidsGameObject.activeSelf);
                    gpuBoidsGameObject.SetActive(true);

                } else if(Input.GetKeyDown(KeyCode.W))
                {
                    gpuBoids.isAudioReactive = !gpuBoids.isAudioReactive;
                }
                 //change 4th stage
                 else if(Input.GetKeyDown(KeyCode.P))
                {
                    ab.enabled = true;
                    fourthSection.SetActive(false);
                    fifthSection.SetActive(true);
                    camAnim.target = fifthSectionCamTarget.transform;
                    curSectionID = 5;
                }
            } else if(curSectionID == 5)
            {
                if(Input.GetKeyDown(KeyCode.Q))
                {
                    pentagonMeshpartcile.SetActive(!pentagonMeshpartcile.activeSelf);
                }
            }
        }
    }
}
