using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Art
{
    public class Anim_WalkingWithBounds3 : AnimTemplate
    {

        public GameObject thing;
        public int amount = 10;
        public Vector3 bounds;
        public float rotateSpeed;

        public override void MakeArt()
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject g = Instantiate(thing, root.transform);
                g.transform.localPosition = Random.insideUnitSphere * .1f;
                g.transform.localEulerAngles = Random.insideUnitSphere * 360;
            }   
        }

		public override void Animate()
		{
            for (int i = 0; i < amount; i++)
            {
                Transform t = root.transform.GetChild(i).transform;
                t.Translate(Vector3.forward * Time.deltaTime*speed);
                Quaternion tempRotation = t.rotation;
                t.LookAt(Vector3.zero,t.up);
                Quaternion lookrotation = t.rotation;
                t.rotation = tempRotation;
                t.rotation = Quaternion.Slerp(t.rotation, lookrotation, Time.deltaTime * rotateSpeed * sdBox(t.position, bounds));
            }
		}

        //https://www.iquilezles.org/www/articles/distfunctions/distfunctions.htm
        float sdBox(Vector3 p, Vector3 b)
        {
            Vector3 q = new Vector3(Mathf.Abs(p.x),Mathf.Abs(p.y),Mathf.Abs(p.z)) - b;
            return Vector3.Magnitude(Vector3.Max(q, Vector3.zero)) + Mathf.Min(Mathf.Max(q.x, Mathf.Max(q.y, q.z)), 0.0f);
        }

	}
}
