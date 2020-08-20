// Copyright 2017 Matt Tytel

using UnityEngine;

namespace AudioHelm
{
    [AddComponentMenu("")]
    public class DestroyAfterTime : MonoBehaviour
    {
        public float time = 10.0f;

        void Start()
        {
            Invoke("Die", time);
        }

        void Die()
        {
            Destroy(gameObject);
        }
    }
}
