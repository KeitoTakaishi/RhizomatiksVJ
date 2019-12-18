using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //public int sceneID = 1;
    public int sectionID = 1;


    #region scene1 public data
    public GameObject cam;
    //CameraAnimation camAnim;
    //PostEffectApply postEffectApply;

    public GameObject firstSection;
    public GameObject wireFarmeSphere;
    public GameObject rythmSpherer;
    public GameObject upperLineTrail;
    public GameObject sphereShapeLineBurst;
    public GameObject boxParticle;
    public GameObject upperQuad;
    public GameObject doom;
    
    Material extrude;

    public GameObject secondSection;
    #endregion

    #region scene2 public data
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
    public GameObject textParticleSyste03;
    public Material defaultSkyBox;
    public Material[] skyBoxMats = new Material[2];
    AudioReactiveText audioReactiveText;
    int skyBoxMode = 0;
    public GameObject polygonParticle;
    public GameObject boxFontManager;
    #endregion

    #region scene4 public data
    public GameObject fourthSection;
    public GameObject walkingModel;
    public GameObject gpuBoidsGameObject;
    GPUBoids gpuBoids;
    #endregion


    #region scene5 public data
    public GameObject fifthSection;
    public GameObject fifthSectionCamTarget;
    public GameObject pentagonMeshpartcile;
    #endregion

    #region private scene data
    Material wireFrameMaterial;
    #endregion

    #region private parameter
    int wireFrameShaderMode = 0;
    int rythmSphereColorMode = 0;
    #endregion

    private void Awake()
    {
        //camAnim = cam.GetComponent<CameraAnimation>();
        //postEffectApply = cam.GetComponent<PostEffectApply>();
        wireFrameMaterial = wireFarmeSphere.GetComponent<MeshRenderer>().material;
        extrude = rythmSpherer.GetComponent<MeshRenderer>().material;
        //sceneID = 1;
        sectionID = 1;

        audioReactiveText = thirdSection.GetComponent<AudioReactiveText>();
        gpuBoids = gpuBoidsGameObject.GetComponent<GPUBoids>();

    }

    void Start()
    {
        
    }

    void Update()
    {



        //effect tyoe-----------------------------------
        if(Input.GetKey(KeyCode.Z))
        {
            //postEffectApply.SwitchMode = SwitchModes.HumanMode;
        } else if(Input.GetKey(KeyCode.X))
        {
            //postEffectApply.SwitchMode = SwitchModes.AutoMode;
        } else if(Input.GetKey(KeyCode.C))
        {
            //postEffectApply.SwitchMode = SwitchModes.MomentaryHumanMode;
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            doom.SetActive(!doom.activeSelf);
        }

        //secetion select-----------------------------------




        //general select-----------------------------------

        if(sectionID == 1)
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                wireFarmeSphere.SetActive(!wireFarmeSphere.activeSelf);

            } else if(Input.GetKeyDown(KeyCode.W))
            {
                wireFrameShaderMode = (wireFrameShaderMode + 1) % 3;
                wireFrameMaterial.SetInt("_mode", wireFrameShaderMode);
            } else if(Input.GetKeyDown(KeyCode.E))
            {
                rythmSpherer.SetActive(!rythmSpherer.activeSelf);
            } else if(Input.GetKeyDown(KeyCode.R))
            {
                rythmSphereColorMode = (rythmSphereColorMode + 1) % 2;
                extrude.SetInt("_colorMode", rythmSphereColorMode);
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
            } else if(Input.GetKeyDown(KeyCode.O))
            {

                //scene切り替え
            } else if(Input.GetKeyDown(KeyCode.P))
            {
                sectionID = 2;
                firstSection.SetActive(false);
                secondSection.SetActive(!secondSection.activeSelf);
                //camAnim.target = humanModelPivot.transform;
                NoiseBox.SetActive(true);

            }
        } else if(sectionID == 2)
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                HeadMesh.SetActive(!HeadMesh.activeSelf);
            } else if(Input.GetKeyDown(KeyCode.W))
            {
                triangleMeshParticle.SetActive(!triangleMeshParticle.activeSelf);

            } else if(Input.GetKeyDown(KeyCode.E))
            {
                prismParticle.SetActive(!prismParticle.activeSelf);

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
                //camAnim.target = trailParticleSyste03.transform;
                sectionID = 3;
                secondSection.SetActive(false);
                prismParticle.SetActive(false);
                triangleMeshParticle.SetActive(false);
                //polygonParticle.SetActive(true);
            }

        } else if(sectionID == 3)
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                doom.SetActive(!doom.activeSelf);

            } else if(Input.GetKeyDown(KeyCode.W))
            {
                skyBoxMode += 1;
                RenderSettings.skybox = skyBoxMats[(skyBoxMode) % 2];
            } else if(Input.GetKeyDown(KeyCode.E))
            {
                boxFontManager.SetActive(!boxFontManager.activeSelf);
            } else if(Input.GetKeyDown(KeyCode.R))
            {
                trailParticleSyste03.SetActive(!trailParticleSyste03.activeSelf);
            } else if(Input.GetKeyDown(KeyCode.T))
            {
                textParticleSyste03.SetActive(!textParticleSyste03.activeSelf);

            } else if(Input.GetKeyDown(KeyCode.Y))
            {
                polygonParticle.SetActive(!polygonParticle.activeSelf);

            } else if(Input.GetKeyDown(KeyCode.M))
            {
                thirdSection.SetActive(false);
                fourthSection.SetActive(true);
                //camAnim.target = walkingModel.transform;
                RenderSettings.skybox = defaultSkyBox;
                polygonParticle.SetActive(false);
                sectionID = 4;
            }
        } else if(sectionID == 4)
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
                fourthSection.SetActive(false);
                fifthSection.SetActive(true);
                //camAnim.target = fifthSectionCamTarget.transform;
                sectionID = 5;
            }
         }
        else if(sectionID == 5)
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                pentagonMeshpartcile.SetActive(!pentagonMeshpartcile.activeSelf);
            }
        }
     }
}
