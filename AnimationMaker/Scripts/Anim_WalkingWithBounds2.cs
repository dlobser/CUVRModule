using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Art
{
    public class Anim_WalkingWithBounds2 : AnimTemplate
    {

        public GameObject thing;
        public int amount = 10;
        Vector3[] directions;
        Vector3[] prevPositions;
        public Vector3 bounds;
        public float noiseAmount;

        public override void MakeArt()
        {
            directions = new Vector3[amount];
            prevPositions = new Vector3[amount];
            for (int i = 0; i < amount; i++)
            {
                GameObject g = Instantiate(thing, root.transform);
                g.transform.localPosition = Random.insideUnitSphere * .5f;
                directions[i] = Random.insideUnitSphere;
            }

        }

		public override void Animate()
		{
            for (int i = 0; i < amount; i++)
            {
                Transform t = root.transform.GetChild(i).transform;
                Vector3 noise = Vector3.zero;

                if (t.localPosition.x > bounds.x || t.localPosition.x < -bounds.x)
                {
                    float newXPosition = t.localPosition.x > bounds.x ? bounds.x : -bounds.x;
                    t.localPosition = new Vector3(newXPosition, t.localPosition.y, t.localPosition.z);
                    directions[i] = new Vector3(directions[i].x * -1, directions[i].y, directions[i].z);
                }

                else if (t.localPosition.y > bounds.y || t.localPosition.y < -bounds.y)
                {
                    float newYPosition = t.localPosition.y > bounds.y ? bounds.y : -bounds.y;
                    t.localPosition = new Vector3(t.localPosition.x, newYPosition, t.localPosition.z);
                    directions[i] = new Vector3(directions[i].x, directions[i].y * -1, directions[i].z);

                }

                else if (t.localPosition.z > bounds.z || t.localPosition.z < -bounds.z)
                {
                    float newZPosition = t.localPosition.z > bounds.z ? bounds.z : -bounds.z;
                    t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, newZPosition);
                    directions[i] = new Vector3(directions[i].x, directions[i].y, directions[i].z * -1);

                }
                else
                {
                    noise = new Vector3(
                        (Mathf.PerlinNoise(t.transform.position.y*5, Time.time) - .5f) * noiseAmount,
                        (Mathf.PerlinNoise(t.transform.position.x*5, Time.time) - .5f) * noiseAmount,
                        (Mathf.PerlinNoise(t.transform.position.z*5, Time.time) - .5f) * noiseAmount);
                }
                
                t.transform.localPosition = t.transform.localPosition + noise + directions[i] * speed * Time.deltaTime;
                t.transform.LookAt(prevPositions[i]);
                if(Vector3.Distance(prevPositions[i],t.transform.localPosition)>.1f)
                    prevPositions[i] = t.transform.localPosition;
            }


		}

	}
}
