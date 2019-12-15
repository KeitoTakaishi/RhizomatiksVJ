using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PostEffect
{
    public class Displacement : MonoBehaviour
    {
        [Range(0.0f, 1.0f)]
        public float power;
        public int type;
        [SerializeField] Texture displacementTexture;
        [SerializeField] Shader shader;
        Material material;
        
        void Start()
        {

            if(material == null)
            {
                material = new Material(shader);
            }

        }

        void Update()
        {

        }
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if(type == 0)
            {
                Shader.EnableKeyword("HORIZONTAL");
                Shader.DisableKeyword("VERTICAL");
            } else if(type == 1)
            {
                Shader.EnableKeyword("VERTICAL");
                Shader.DisableKeyword("HORIZONTAL");
            }
            material.SetFloat("power", power);
            material.SetTexture("displacementTex", displacementTexture);
            Graphics.Blit(source, destination, material);
        }
    }
}
