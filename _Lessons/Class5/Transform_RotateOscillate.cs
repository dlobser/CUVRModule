using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transform_RotateOscillate : Transform_Generic
{
    //public float speed = 1;
    public float rotationFrequency = 1;
    public float rotationAmplitude = 30;
    public float rotationPhase = 0;

    Vector3 rotation = Vector3.zero;
    
    void Start()
    {
        
    }

    void Update()
    {
        float rotationZ = Mathf.Sin(Time.time * rotationFrequency * speed + rotationPhase) * rotationAmplitude;
        rotation.Set(0, 0, rotationZ);
        this.transform.localEulerAngles = rotation;
    }
}
