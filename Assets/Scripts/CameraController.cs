using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    #region private data
    Camera camera;
    [SerializeField] Transform target;
    [SerializeField] int moveTimeLength = 60; //何秒かけて目的地まで行くか
    [SerializeField] float radius;
    int curTime = 0;
    Vector3 position;
    Vector3 startPosition;
    Vector3 nextPosition;
    Vector3 dir;
    bool isMoving = false;
    float t, v = 0;
    [SerializeField] EasingType type;
    [SerializeField] bool isUp = false;
    #endregion

    Easing easing;
   
    public enum EasingType
    {
        easeInQuad,
        easeOutQuad,
        easeInOutQuad,
        easeInCubic,
        easeOutCubic,
        easeInOutCubic,
        easeInExpo,
        easeOutExpo,
        easeInOutExpo
    }


    void Awake()
    {
        camera = this.GetComponent<Camera>();
        easing = this.GetComponent<Easing>();
    }

    void Start()
    {
        
    }

    void Update()
    {

        if(isMoving == false)
        {
            if(Input.GetKeyDown(KeyCode.A))
            {
                isMoving = true;
                t = v = 0;
                curTime = 0;
                startPosition = camera.transform.position;
                nextPosition = nextPos(radius);
                dir = nextPosition - startPosition;

            }
        }

        
        if(isMoving)
        {
            t = normalizeTime(curTime, moveTimeLength);

            if(type == EasingType.easeInQuad)
            {
                v = easing.easeInQuad(t);
            }else if(type == EasingType.easeOutQuad)
            {
                v = easing.easeOutQuad(t);
            }else if(type == EasingType.easeInOutQuad)
            {
                v = easing.easeInOutQuad(t);
            }else if(type == EasingType.easeInCubic)
            {
                v = easing.easeInCubic(t);
            } else if(type == EasingType.easeOutCubic)
            {
                v = easing.easeOutCubic(t);
            } else if(type == EasingType.easeInOutCubic)
            {
                v = easing.easeInOutCubic(t);
            } else if(type == EasingType.easeInExpo)
            {
                v = easing.easeInExpo(t);
            } else if(type == EasingType.easeOutExpo)
            {
                v = easing.easeOutExpo(t);
            } else if(type == EasingType.easeInOutExpo)
            {
                v = easing.easeInOutExpo(t);
            }



            curTime++;
            if(t > 1.0) isMoving = false;
        }

        camera.transform.position = dir * v + startPosition;
        camera.transform.LookAt(target, Vector3.up);
    }


    
    float normalizeTime(float cur, float length)
    {
        float t = cur / length;
        return t;
    }

    Vector3 nextPos(float radius)
    {

        var nextPos = Random.insideUnitSphere * radius;

        if(isUp)
        {
            while(nextPos.y <= 0.0)
            {
                nextPos = Random.insideUnitSphere * radius;
            }
        }

        return nextPos;
    }

}
