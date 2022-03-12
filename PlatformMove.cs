using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    float dir = 0.02f;
    float max;
    float min;


    void Start()
    {
        max = transform.localPosition.x + 1.5f;
        min = transform.localPosition.x - 1.5f;
    }

    void Update()
    {


        if(transform.localPosition.x >= max )
        {
            dir = -0.02f;
        }
        else if(transform.localPosition.x <= min)
        {
            dir = 0.02f;
        }


        transform.Translate(Vector3.right * dir,Space.Self);
       
    }
}
