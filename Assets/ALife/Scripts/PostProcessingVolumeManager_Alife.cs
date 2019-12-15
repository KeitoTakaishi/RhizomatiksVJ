using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class PostProcessingVolumeManager_Alife : MonoBehaviour
{
    [SerializeField]
    float maxIntencity = 3.0f;

    [SerializeField]
    PostProcessVolume postProcessVolume;
    private PostProcessProfile postProcessProfile;
    Bloom bloom;

    void Start()
    {
        postProcessProfile = postProcessVolume.profile;
        bloom = postProcessProfile.GetSetting<Bloom>();
        bloom.enabled.Override(true);
    }

    void Update()
    {
        if(OscData.rythm == 1.0)
        {
            bloom.intensity.Override(Random.Range(0.0f, maxIntencity));
        } else
        {
            bloom.intensity.Override(1.0f);
        }

    }

    void OnDestroy()
    {       
        RuntimeUtilities.DestroyProfile(postProcessProfile, true);
    }
}
