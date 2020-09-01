using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Art
{
    public class Anim : MonoBehaviour
    {
        public bool rebuild;
        public float counter { get; set; }
        public float speed = 1;
        public virtual void Rebuild() { }
        public virtual void Animate() { }
    }
}
