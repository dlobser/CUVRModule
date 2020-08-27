using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR
{
    public class ArtMakerNoise : ArtMakerTemplate
    {
        public int amount = 10;
        public int length = 10;
        public float frequency;
        public float multiply = .1f;
        public Vector3 offset;
        public float hue;
        public float width;
        public bool randomize;
        float Hue;
        float hueOffset;

        public override void MakeArt()
        {
            if (randomize)
            {
                offset = Random.insideUnitSphere * 360;
                amount = Random.Range(5, 1000);
                length = Random.Range(30, 100);
                width = Random.Range(.01f, .02f);
                frequency = Random.Range(.5f, 3f);
                hue = Random.value;
                hueOffset = Random.value;
            }

            for (int i = 0; i < amount; i++)
            {

                GameObject l = new GameObject();
                l.transform.parent = root.transform;
                l.transform.localPosition = Random.insideUnitSphere * .5f;

                LineRenderer line = l.AddComponent<LineRenderer>();
                line.positionCount = length;
                line.material = new Material(Shader.Find("Standard"));
                line.widthMultiplier = width;
                line.widthCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(.3f, 1), new Keyframe(1, 0));

                float rando = ((1 + Random.value) * 1000);
                float off = ((1 - l.transform.localPosition.z) + .5f);
                line.material.color = Color.HSVToRGB((hue + off * hueOffset) % 1, .8f, off);

                for (int j = 0; j < length; j++)
                {
                    Vector3 v = l.transform.localPosition * frequency;
                    Vector3 p = curlNoise(l.transform.position);
                    l.transform.Translate(p * .01f);
                    line.SetPosition(j, l.transform.localPosition);
                }

            }

        }

        Vector3 curlNoise(Vector3 position)
        {
            float morphOffset = .01f;
            Vector3 point = new Vector3(position.z, position.y, position.x + morphOffset) + offset;
            Vector3 sampleX = Noise.Perlin3D(point, frequency).derivative;
            sampleX *= multiply;
            point = new Vector3(position.x + 100f, position.z, position.y + morphOffset) + offset;
            Vector3 sampleY = Noise.Perlin3D(point, frequency).derivative;
            sampleY *= multiply;
            point = new Vector3(position.y, position.x + 100f, position.z + morphOffset) + offset;
            Vector3 sampleZ = Noise.Perlin3D(point, frequency).derivative;
            sampleZ *= multiply;
            Vector3 curl;
            curl.x = sampleZ.x - sampleY.y;
            curl.y = sampleX.x - sampleZ.y;
            curl.z = sampleY.x - sampleX.y;
            return curl;
        }

    }
}