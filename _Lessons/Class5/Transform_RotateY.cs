using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transform_RotateY : Transform_Generic
{
    //public float speed = 1;
    float counter;
    Vector3 rotation = Vector3.zero;

    void Start()
    {
        
    }

    void Update()
    {
        counter += Time.deltaTime * speed;
        rotation.Set(0, counter, 0);
        this.transform.localEulerAngles = rotation;
    }
}
