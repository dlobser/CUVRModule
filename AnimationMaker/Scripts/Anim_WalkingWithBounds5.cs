using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Art
{
    public class Anim_WalkingWithBounds5 : AnimTemplate
    {

        public GameObject thing;
        public int amount = 10;
        public Vector3 bounds;

        public override void MakeArt()
        {

            for (int i = 0; i < amount; i++)
            {
                GameObject g = Instantiate(thing, root.transform);
                g.transform.localPosition = Random.insideUnitSphere * .1f;
            }   
        }

		public override void Animate()
		{
            for (int i = 0; i < amount; i++)
            {
                Transform t = root.transform.GetChild(i);
                t.localPosition = t.position + Random.insideUnitSphere * speed;

                if (t.localPosition.x > bounds.x)
                {
                    t.localPosition = new Vector3(bounds.x, t.localPosition.y, t.localPosition.z);
                }
                if (t.localPosition.x < -bounds.x)
                {
                    t.localPosition = new Vector3(-bounds.x, t.localPosition.y, t.localPosition.z);
                }
                if (t.localPosition.y > bounds.y)
                {
                    t.localPosition = new Vector3(t.localPosition.x, bounds.y, t.localPosition.z);
                }
                if (t.localPosition.y < -bounds.y)
                {
                    t.localPosition = new Vector3(t.localPosition.x, -bounds.y, t.localPosition.z);
                }
                if (t.localPosition.z > bounds.z)
                {
                    t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, bounds.z);
                }
                if (t.localPosition.z < -bounds.z)
                {
                    t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, -bounds.z);
                }
            }
		}
	}
}
