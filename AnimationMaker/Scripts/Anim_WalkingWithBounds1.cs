using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Art
{
    public class Anim_WalkingWithBounds1 : AnimTemplate
    {

        public GameObject thing;
        public int amount = 10;
        public float rotationSpeed;
        public Vector3 bounds;
        public float noiseAmount;

        public override void MakeArt()
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject g = Instantiate(thing, root.transform);
                g.transform.localPosition = Random.insideUnitSphere * .5f;
                g.transform.localEulerAngles = Random.insideUnitSphere * 360;
            }

        }

		public override void Animate()
		{
            for (int i = 0; i < amount; i++)
            {
                Transform t = root.transform.GetChild(i).transform;

                if (t.localPosition.x > bounds.x || t.localPosition.x < -bounds.x)
                {
                    float newXPosition = t.localPosition.x > bounds.x ? bounds.x : -bounds.x;
                    t.localPosition = new Vector3(newXPosition, t.localPosition.y, t.localPosition.z);
                    t.LookAt(GetReflection(t,Vector3.right));
                }

                else if (t.localPosition.y > bounds.y || t.localPosition.y < -bounds.y)
                {
                    float newYPosition = t.localPosition.y > bounds.y ? bounds.y : -bounds.y;
                    t.localPosition = new Vector3(t.localPosition.x, newYPosition, t.localPosition.z);
                    t.LookAt(GetReflection(t, Vector3.up));

                }

                else if (t.localPosition.z > bounds.z || t.localPosition.z < -bounds.z)
                {
                    float newZPosition = t.localPosition.z > bounds.z ? bounds.z : -bounds.z;
                    t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, newZPosition);
                    t.LookAt(GetReflection(t,Vector3.forward));
                }
                t.transform.Rotate(
                    (Mathf.PerlinNoise(t.transform.position.y * 5, Time.time)-.5f) * noiseAmount,
                    (Mathf.PerlinNoise(t.transform.position.x * 5, Time.time)-.5f) * noiseAmount, 0);
                t.Translate(Vector3.forward * speed * Time.deltaTime);
            }
		}

        Vector3 GetReflection(Transform t, Vector3 normal){
            Ray ray = new Ray(t.localPosition, t.forward);
            Vector3 dir = Vector3.Reflect(ray.direction, normal);
            return t.localPosition + dir;
        }
	}
}
