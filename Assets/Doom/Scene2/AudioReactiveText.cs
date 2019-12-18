using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReactiveText : MonoBehaviour
{
    public GameObject Burn, Now;
    public GameObject[] Burns;
    public GameObject[] Nows;
    public GameObject inputManager;
    int num = 30;
    float height = 5000.0f;

    public GameObject MainText;

    public float low;
    public float kick;
    public bool isAudioReactive = false;
    public float radius = 30.0f;

    void Start()
    {
        Burns = new GameObject[30];
        Nows = new GameObject[30];

        for(int i = 0; i < num; i++)
        {
            //var pos = Random.insideUnitCircle * 50.0f;
            var r = radius;
            //var 
            var pos = new Vector2(50.0f * Mathf.Cos( (i *360.0f/num)*Mathf.Deg2Rad ), 50.0f * Mathf.Sin((i * 360.0f / num) * Mathf.Deg2Rad));
            var offSetY = Random.Range(-10.0f, 10.0f);
            //var eular = new Vector3(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
            //Burns[i] = (GameObject)Instantiate(Burn, new Vector3(pos.x, height - offSetY, pos.y), Quaternion.Euler(eular));
            Burns[i] = (GameObject)Instantiate(Burn, new Vector3(pos.x, height - offSetY, pos.y), Quaternion.identity);
            Burns[i].transform.LookAt(MainText.transform);
            Burns[i].transform.parent = this.transform;
            //pos = Random.insideUnitCircle * 30.0f;
            offSetY = Random.Range(0.0f, 30.0f);
            //eular = new Vector3(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));

            //Nows[i] = (GameObject)Instantiate(Now, new Vector3(pos.x, height - offSetY, pos.y), Quaternion.Euler(eular));
            Nows[i] = (GameObject)Instantiate(Now, new Vector3(pos.x, height - offSetY, pos.y), Quaternion.identity);
            Nows[i].transform.LookAt(MainText.transform);
            Nows[i].transform.parent = this.transform;
        }
    }

    void Update()
    {

        if(inputManager.GetComponent<InputManager>().sectionID == 3)
        {
            if(isAudioReactive)
            {
                for(int i = 0; i < num; i++)
                {
                    if(i % 2 == 0)
                    {
                        Burns[i].transform.localScale = new Vector3(low, low, low);
                        Nows[i].transform.localScale = new Vector3(low, low, low);
                    } else
                    {
                        if(kick > 0.0f)
                        {
                            var rand = Random.Range(0.0f, 1.0f);
                            Burns[i].transform.localScale = new Vector3(1.5f + rand, 1.5f + rand, 1.5f + rand);
                            Nows[i].transform.localScale = new Vector3(1.5f + rand, 1.5f + rand, 1.5f + rand);
                        }
                    }

                }
                float lowOff = 1.0f;
                MainText.transform.localScale = new Vector3(3.0f + low * lowOff, 3.0f + low * lowOff, 3.0f + low * lowOff);
            }
        }
    }
}
