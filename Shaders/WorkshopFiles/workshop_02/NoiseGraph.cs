using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGraph : MonoBehaviour
{
    LineRenderer rend;
    public int detail;

    public float frequency;
    public float value;
    public float offset;
    public float permute;

    public bool abs;

    void Start()
    {
        rend = this.gameObject.AddComponent<LineRenderer>();
        rend.positionCount =detail;
    }

    void Update()
    {
        for (int i = 0; i < detail; i++)
        {
            rend.SetPosition(i, new Vector3(i, Mathf.PerlinNoise(offset + i * frequency, permute) * value));
        }
    }
}
