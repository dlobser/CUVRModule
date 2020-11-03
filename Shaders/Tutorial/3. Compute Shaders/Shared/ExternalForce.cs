using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalForce : MonoBehaviour
{
    [System.Serializable]
    public struct ForceData
    {
        public Vector3 Position;
        public float Force;
        public float Range;

        public static int Stride = 5 * sizeof(float);
    }

    [SerializeField]
    float _force;

    [SerializeField]
    float _range = 1f;

    public float Force { get { return _force; } }

    public ForceData Data
    {
        get
        {
            return new ForceData
            {
                Position = transform.position,
                Force = _force,
                Range = _range
            };
        }
    }

}
