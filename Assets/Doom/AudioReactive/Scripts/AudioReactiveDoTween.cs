using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioReactiveDoTween : MonoBehaviour
{
    #region private data
    [SerializeField] int tweenPropertyType = 0;
    [SerializeField] float minRotateSpan = 0.1f;
    [SerializeField] float maxRotateSpan = 0.5f;
    [SerializeField] float scaleSpan = 0.5f;
    private bool isRotateCamplete = true;
    private bool isScaleCamplete = true;
    private int kickDitection;
    #endregion

    #region accesor
    public bool RotateCamplete
    {
        get {
            return isRotateCamplete;
        }
    }

    public int KickDetection
    {
        get {
            return kickDitection;
        }
        set {
            kickDitection = value;
        }
    }
    public 
    #endregion

    void Start()
    {
        
    }

    void Update()
    {
        //auto change
        var time = Time.realtimeSinceStartup;
        time = time % 30;
        if(time < 15)
        {
            tweenPropertyType = 0;
        } else
        {
            tweenPropertyType = 1;
        }

        if(tweenPropertyType == 0)
        {
            if(kickDitection == 1 && isRotateCamplete)
            {
                TweenRotate();
            }
        }else if(tweenPropertyType == 1)
        {
            if(kickDitection == 1 && isScaleCamplete)
            {
                TweenScale();
            }
        }
    }

    void TweenRotate()
    {
       
        isRotateCamplete = false;
        var mode = UnityEngine.Random.Range(0f, 1f);
        float rotateVal = 90.0f;
        var rotateSpan = UnityEngine.Random.Range(minRotateSpan, maxRotateSpan);
        if(mode < 0.3f)
        {
            
            transform.DOLocalRotate(new Vector3(rotateVal, 0, 0), rotateSpan)
            .SetEase(Ease.Linear)
            .OnComplete(() => isRotateCamplete = true)
            .SetRelative();
        }else if(mode < 0.3f && mode > 0.6f)
        {
            transform.DOLocalRotate(new Vector3(rotateVal, 0, 0), rotateSpan)
           .SetEase(Ease.Linear)
            .OnComplete(() => isRotateCamplete = true)
            .SetRelative();
        } else
        {
            transform.DOLocalRotate(new Vector3(0, 0, rotateVal), rotateSpan)
          .SetEase(Ease.Linear)
           .OnComplete(() => isRotateCamplete = true)
           .SetRelative();
        }

    }

    void TweenScale()
    {

        isScaleCamplete = false;
        var mode = UnityEngine.Random.Range(0f, 1f);
        var expand = UnityEngine.Random.Range(5.0f, 40.0f);
        if(mode <  0.3f)
        {
            var seq = DOTween.Sequence();
            seq.Append(
                transform.DOScaleY(expand, scaleSpan)
                .SetEase(Ease.Linear)
                .SetRelative()
                );

            seq.Append(
               transform.DOScaleY(-expand, 0.1f)
               .SetEase(Ease.Linear)
               .SetRelative()
               .OnComplete(()=> isScaleCamplete = true)
               );
        }
        else if(mode > 0.3f && mode < 0.6f)
        {
            var seq = DOTween.Sequence();
            seq.Append(
                transform.DOScaleX(expand, scaleSpan)
                .SetEase(Ease.Linear)
                .SetRelative()
                );

            seq.Append(
               transform.DOScaleX(-expand, scaleSpan)
               .SetEase(Ease.Linear)
               .SetRelative()
               .OnComplete(() => isScaleCamplete = true)
               );
        } else
        {
            var seq = DOTween.Sequence();
            seq.Append(
                transform.DOScaleZ(expand, scaleSpan)
                .SetEase(Ease.Linear)
                .SetRelative()
                );

            seq.Append(
               transform.DOScaleZ(-expand, scaleSpan)
               .SetEase(Ease.Linear)
               .SetRelative()
               .OnComplete(() => isScaleCamplete = true)
               );
        }

      
       
    }

    void TweenScaleComp()
    {
        //this.transform.localScale = 
        isScaleCamplete = true;

    }
}