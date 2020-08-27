using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR
{
    public class ArtSquareTube : ArtMakerTemplate
    {
        public int detail = 100;
		public override void MakeArt()
		{
            GameObject g = new GameObject();
            g.transform.SetParent(root.transform);
            TubeRenderer tube = g.AddComponent<TubeRenderer>();

            Vector3[] vecs = new Vector3[detail];
            float[] radius = new float[detail];
            float r = Random.value*1000;
            float mult = Random.Range(.0005f, .02f);

            for (int i = 0; i < detail; i++)
            {
                float j = (float)i*mult+r;
                vecs[i] = new Vector3(
                    Mathf.PerlinNoise(j*4.3321f,j)-.5f,
                    Mathf.PerlinNoise(j,j*3.342f)-.5f,
                    Mathf.PerlinNoise(j * 4.3321f, j*.345f) - .5f)*2;
                radius[i] = (Mathf.Cos(((float)i/(float)detail)*Mathf.PI*2)-1)*-.015f;

            }

            tube.SetPoints(vecs, radius, Color.white);
            tube.material = new Material(Shader.Find("Standard"));

            GameObject g2 = Instantiate(g);
            g2.transform.localScale = new Vector3(-1, 1, 1);
            g2.transform.SetParent(g.transform);

            for (int i = 1; i < 4; i++)
            {
                GameObject b = Instantiate(g);
                b.transform.localEulerAngles = new Vector3(0, 0, i * 90);
                b.transform.SetParent(root.transform);
            }
        }
	}

}