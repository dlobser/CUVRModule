using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayObjects : MonoBehaviour
{
    public GameObject sprayObject;
    public float rate;
    public float spread;
    float counter = 0;

    void Start()
    {
        
    }

    void Update()
    {
        counter += Time.deltaTime;
        if(counter>1/rate){
            counter = 0;
            Instantiate(sprayObject, this.transform.position + Random.insideUnitSphere * spread, Quaternion.identity);

        }
    }
}
