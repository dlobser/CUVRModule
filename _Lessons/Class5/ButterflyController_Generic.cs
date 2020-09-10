using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyController_Generic : MonoBehaviour
{
    public float speed;
    public Transform_Generic[] generics;

    void Start()
    {
        generics = GetComponentsInChildren<Transform_Generic>();
    }

    void Update()
    {
        for (int i = 0; i < generics.Length; i++)
        {
            generics[i].speed = speed;
        }
    }
}
