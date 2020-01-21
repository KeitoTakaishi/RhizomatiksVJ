using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopLayerSin : MonoBehaviour
{
    //public Vector2 center = new Vector2(0.5f, 0.5f);
    [Range(0, 100)]
    public float power = 0f;
    [SerializeField]
    Material material;

    [SerializeField] float maxradiationBlurPower;
    [SerializeField] float radiationBlurEffectTime;
    void Start()
    {
        
    }

    void Update()
    {

        //material.SetVector("_BlurCenter", center);
        material.SetFloat("Power", power);
        if(OscData.kick == 1)
        //if(Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine("RadiationSin");
        }

        
    }

    IEnumerator RadiationSin()
    {
        float duration = radiationBlurEffectTime;
        while(duration > 0f)
        {
            duration = Mathf.Max(duration - Time.deltaTime, 0);
            power = maxradiationBlurPower * Easing.easeInQuad(duration / radiationBlurEffectTime);
            yield return null;
        }
    }
}
