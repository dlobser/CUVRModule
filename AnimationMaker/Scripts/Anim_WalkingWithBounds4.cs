using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Art
{
    public class Anim_WalkingWithBounds4 : AnimTemplate
    {

        public GameObject thing;
        public int amount = 10;
        public Vector3 bounds;

        Vector3[] oldPositions;
        Vector3[] newPositions;
        float[] lerps;

        public override void MakeArt()
        {
            newPositions = new Vector3[amount];
            oldPositions = new Vector3[amount];
            lerps = new float[amount];

            for (int i = 0; i < amount; i++)
            {
                GameObject g = Instantiate(thing, root.transform);
                g.transform.localPosition = Random.insideUnitSphere * .1f;
                oldPositions[i] = g.transform.localPosition;
                g.transform.localEulerAngles = Random.insideUnitSphere * 360;
            }   
        }

		public override void Animate()
		{
            for (int i = 0; i < amount; i++)
            {
                Transform t = root.transform.GetChild(i);

                float distance = Vector3.Distance(oldPositions[i], newPositions[i]);
                t.LookAt(newPositions[i]);

                t.transform.localPosition = Vector3.Lerp(oldPositions[i], newPositions[i], lerps[i]);
                lerps[i] += (Time.deltaTime * speed) * (1/distance);

                if (lerps[i] >= 1)
                {
                    oldPositions[i] = newPositions[i];
                    newPositions[i] = new Vector3(
                        Random.Range(-bounds.x, bounds.x),
                        Random.Range(-bounds.y, bounds.y),
                        Random.Range(-bounds.z, bounds.z)
                    );
                    lerps[i] = 0;

                }
            }
		}

	}
}
