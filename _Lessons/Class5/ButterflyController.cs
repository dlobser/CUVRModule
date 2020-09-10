using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyController : MonoBehaviour
{
    public float speed;
    public Transform_RotateY RotateY;
    public Transform_TranslateYOscillate TranslateYOscillate;
    public Transform_RotateOscillate RotateOscillate1;
    public Transform_RotateOscillate RotateOscillate2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotateY.speed = speed;
        TranslateYOscillate.speed = speed;
        RotateOscillate1.speed = speed;
        RotateOscillate2.speed = speed;
    }
}
