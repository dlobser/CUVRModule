using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transform_TranslateYOscillate : Transform_Generic
{
    //public float speed = 1;
    public float transformFrequency = 1;
    public float transformAmplitude = 30;
    public float transformPhase = 0;

    Vector3 translate = Vector3.zero;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float transformY = Mathf.Sin(Time.time * transformFrequency * speed + transformPhase) * transformAmplitude;
        translate.Set(0, transformY, 0);
        this.transform.localPosition = translate;
    }
}
