using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleUpOnStart : MonoBehaviour
{
    public float speed;
    float counter;
    Vector3 initScale;

    void Awake()
    {
        initScale = this.transform.localScale;
        this.transform.localScale = Vector3.zero;
    }

    void Update()
    {
        if(counter<speed){
            counter += Time.deltaTime;
            this.transform.localScale = Vector3.Lerp(Vector3.zero, initScale, Mathf.SmoothStep(0,1,counter / speed));
        }
    }
}
