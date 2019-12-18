using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class singleton : MonoBehaviour
{
    static bool exist = false;

    private void Awake()
    {
        if(exist)
        {
            Destroy(gameObject);
            return;
        }
        exist = true;
        DontDestroyOnLoad(gameObject);

        Cursor.visible = false;

    }
}
