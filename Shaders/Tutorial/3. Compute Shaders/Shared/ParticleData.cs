using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ParticleData
{
    public Vector3 Position;
    public Vector3 Velocity;
    public float Mass;
    public float Age;
    public float Life;


    public static int stride = 9 * sizeof(float);
}
