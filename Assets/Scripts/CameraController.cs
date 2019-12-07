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
    [SerializeField] GameObject midiController;
    MidiReciever midi;
   
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
        startPosition = this.transform.position;
        camera.transform.position = dir * v + startPosition;
        camera.transform.LookAt(target, Vector3.up);
        midi = midiController.GetComponent<MidiReciever>();
    }

    void Update()
    {

        if(isMoving == false)
        {
            if(midi.notes[0] ||Input.GetKeyDown(KeyCode.A) || (target.position - this.transform.position).magnitude > 11f)
            {
                isMoving = true;
                t = v = 0;
                curTime = 0;
                startPosition = camera.transform.position;

                if(Random.Range(0.0f, 1.0f) < 0.95f) {
                    nextPosition = SpereRandomNextPos(radius);
                } else
                {
                    nextPosition = ForwardNextPos();
                }
                

                dir = nextPosition - startPosition;

            }

            
            var time = Time.realtimeSinceStartup;
            var vel = new Vector3(Mathf.PerlinNoise(this.transform.position.x, time), 
                Mathf.PerlinNoise(this.transform.position.y, time)*0.04f,
                Mathf.PerlinNoise(this.transform.position.z, time))*0.04f;
            this.transform.position += vel;
            this.transform.LookAt(target);
            
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
            camera.transform.position = dir * v + startPosition;
            camera.transform.LookAt(target, Vector3.up);
        }

        
    }


    
    float normalizeTime(float cur, float length)
    {
        float t = cur / length;
        return t;
    }

    Vector3 SpereRandomNextPos(float radius)
    {
        
        var nextPos = Random.insideUnitSphere;
        
        if(isUp)
        {
            while(nextPos.y <= 0.0f)
            {
                nextPos = Random.insideUnitSphere;
            }   
        }
        nextPos = nextPos * radius + target.transform.position;
        return nextPos;
    }

    Vector3 ForwardNextPos()
    {
        var ratio = Random.Range(0.0f, 0.5f);
        var nextPos = (target.transform.position - this.transform.position) * ratio + startPosition;
        return nextPos;
    }

}
