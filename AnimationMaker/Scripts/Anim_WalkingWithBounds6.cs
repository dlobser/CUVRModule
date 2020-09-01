using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Art
{
    public class Anim_WalkingWithBounds6 : AnimTemplate
    {

        public GameObject thing;
        public int amount = 10;
        public Vector3 bounds;

        Vector3[] oldPositions;
        Vector3[] newPositions;
        Vector3[] oldAngles;
        Vector3[] newAngles;
        float[] lerps;

        public override void MakeArt()
        {
            newPositions = new Vector3[amount];
            oldPositions = new Vector3[amount];
            oldAngles = new Vector3[amount];
            newAngles = new Vector3[amount];
            lerps = new float[amount];

            for (int i = 0; i < amount; i++)
            {
                GameObject g = Instantiate(thing, root.transform);
                g.transform.localPosition = Random.insideUnitSphere * .1f;
                oldPositions[i] = g.transform.localPosition;
                oldAngles[i] = Random.insideUnitSphere;
                g.transform.localEulerAngles = Random.insideUnitSphere * 360;
            }
        }

        public override void Animate()
        {
            for (int i = 0; i < amount; i++)
            {
                Transform t = root.transform.GetChild(i);

                float distance = 
                    Vector3.Distance(oldPositions[i], oldPositions[i] + oldAngles[i]) + 
                    Vector3.Distance(oldPositions[i] + oldAngles[i], newPositions[i] + newAngles[i] * -1 ) + 
                    Vector3.Distance(newPositions[i], newPositions[i] + newAngles[i]*-1);

                Vector3 look = CubicSpline(
                    oldPositions[i],
                    oldPositions[i] + oldAngles[i],
                    newPositions[i] + newAngles[i] * -1,
                    newPositions[i],
                    lerps[i]+.01f);

                t.transform.localPosition = CubicSpline(
                    oldPositions[i],
                    oldPositions[i] + oldAngles[i],
                    newPositions[i] + newAngles[i] * -1,
                    newPositions[i],
                    lerps[i]);

                t.LookAt(look);

                lerps[i] += (Time.deltaTime * speed) * (1 / distance);

                if (lerps[i] >= 1)
                {
                    oldAngles[i] = newAngles[i];
                    newAngles[i] = Random.insideUnitSphere*.5f*Vector3.Distance(newPositions[i],oldPositions[i]);
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

        Vector3 CubicSpline(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
        {
            Vector3 A = Vector3.Lerp(a, b, t);
            Vector3 B = Vector3.Lerp(b, c, t);
            Vector3 C = Vector3.Lerp(c, d, t);
            Vector3 A2 = Vector3.Lerp(A, B, t);
            Vector3 B2 = Vector3.Lerp(B, C, t);
            return Vector3.Lerp(A2, B2, t);
        }

    }
}
