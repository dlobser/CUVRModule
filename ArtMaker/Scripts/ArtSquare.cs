using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR
{
    public class ArtSquare : ArtMakerTemplate
    {
        /*Instantiate a sphere or a cube near the center
         * Create a spine, offset it and aim it at a corner
         * along the spine, create 3 to 7 random cubes rotated perpendicularly to the spine
         * scatter some random shapes around
         */

        public GameObject[] elements;
        public Color[] colors;

        public int amount;

        public float minScale;
        public float maxScale;


        public override void Rebuild()
        {
            if (root != null)
                Destroy(root);

            root = new GameObject();

            int index = Random.Range(0, elements.Length);
            GameObject g = Instantiate(elements[index]);
            g.transform.parent = root.transform;
            g.transform.localPosition = new Vector3(Random.Range(-.15f, .15f), Random.Range(-.15f, .15f), 2);
            float s = Random.Range(.4f, .6f);
            g.transform.localScale = new Vector3(s, s, s);
            g.GetComponent<MeshRenderer>().material.color = colors[Random.Range(0, colors.Length)];
            g.transform.localEulerAngles = new Vector3(0, Random.Range(-10, 10), 0);

            GameObject spine = Instantiate(elements[1]);
            spine.transform.localPosition = new Vector3(Random.Range(-.15f, .15f), Random.Range(-.15f, .15f), 0);
            float spineRotation = Random.value * 360;
            spine.transform.localEulerAngles = new Vector3(0, 0, spineRotation);
            spine.transform.parent = root.transform;
            spine.GetComponent<MeshRenderer>().material.color = colors[Random.Range(0, colors.Length)];

            int a = Random.Range(1, amount);
            for (int i = 0; i < a; i++)
            {
                GameObject element = Instantiate(elements[1]);
                element.transform.parent = spine.transform;
                element.transform.localPosition = new Vector3(0, Random.Range(-.5f, .5f), Random.value);
                element.transform.localEulerAngles = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10f, 10f));
                element.transform.localScale = new Vector3(Random.Range(.1f, .8f) * (.7f + element.transform.localPosition.y), Random.Range(.02f, .15f), Random.value * .1f);
                element.transform.parent = root.transform;
                element.GetComponent<MeshRenderer>().material.color = colors[Random.Range(0, colors.Length)] * Random.value;

            }
            spine.transform.localScale = new Vector3(Random.Range(.01f, .04f), spine.transform.localScale.y, Random.value * .1f);

            if (Random.value > .5f)
            {
                spine = Instantiate(elements[1]);
                spine.transform.localPosition = new Vector3(Random.Range(-.15f, .15f), Random.Range(-.15f, .15f), 0);
                spine.transform.localEulerAngles = new Vector3(0, 0, spineRotation + Random.Range(1, 10));
                spine.transform.parent = root.transform;
                spine.GetComponent<MeshRenderer>().material.color = colors[Random.Range(0, colors.Length)];

                for (int i = 0; i < a * .5f; i++)
                {
                    GameObject element = Instantiate(elements[1]);
                    element.transform.parent = spine.transform;
                    element.transform.localPosition = new Vector3(0, Random.Range(-.5f, .5f), Random.value);
                    element.transform.localEulerAngles = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10f, 10f));
                    element.transform.localScale = new Vector3(Random.Range(.1f, .4f), Random.Range(.02f, .15f), Random.value * .1f);
                    element.transform.parent = root.transform;
                    element.GetComponent<MeshRenderer>().material.color = colors[Random.Range(0, colors.Length)];

                }
                spine.transform.localScale = new Vector3(Random.Range(.01f, .04f), spine.transform.localScale.y, Random.value * .1f);
            }

            for (int i = 0; i < 10; i++)
            {
                index = Random.Range(0, elements.Length);
                GameObject element = Instantiate(elements[index]);
                element.transform.parent = root.transform;
                element.transform.localPosition = new Vector3(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f), index == 0 ? -1 : -2);
                element.transform.localEulerAngles = new Vector3(0, 0, Random.value * 360);
                s = Random.Range(.02f, .15f);
                element.transform.localScale = new Vector3(s, s, s);
                element.GetComponent<MeshRenderer>().material.color = colors[Random.Range(0, colors.Length)];
                if (index > 0)
                {
                    element.transform.localScale = new Vector3(s, s * .2f, s);
                    int amt = Random.Range(0, 6);
                    float rotateAmount = Random.value;
                    for (int j = 0; j < amt; j++)
                    {
                        GameObject d = Instantiate(element);
                        Color color = d.GetComponent<MeshRenderer>().material.color * Random.Range(.5f, 1);
                        d.GetComponent<MeshRenderer>().material.color = color;
                        d.transform.Translate(Vector3.down * (.031f * (float)j));
                        d.transform.Translate(Vector3.right * .04f * j * ((1 + Mathf.Sin(j)) * .2f));
                        d.transform.localScale = new Vector3((d.transform.localScale.x * Random.Range(.8f, 1)) * ((amt - (j * .5f)) / amt), d.transform.localScale.y, d.transform.localScale.z);
                        d.transform.Rotate(Vector3.forward * 10 * j * rotateAmount);
                        d.transform.parent = root.transform;
                    }

                }

            }
        }
    }
}