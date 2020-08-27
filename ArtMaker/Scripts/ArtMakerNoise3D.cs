using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR
{
    public class ArtMakerNoise3D : ArtMakerTemplate
    {
        public GameObject thing;
        public int amount = 10;
        public float frequency;
        public float multiply = .1f;
        public Vector3 offset;
        public bool keepRebuilding;

        public override void MakeArt()
        {
            for (int i = 0; i < amount; i++)
            {
                for (int j = 0; j < amount; j++)
                {
                    for (int k = 0; k < amount; k++)
                    {
                        GameObject g = Instantiate(thing, new Vector3(i, j, k), Quaternion.identity, root.transform);
                        g.transform.parent = root.transform;
                        float s = Noise.Noise3D(new Vector3(i, j, k) * frequency + offset);
                        g.transform.localScale = Vector3.one * s;
                    }
                }
            }

        }

        void Update()
        {
            if (keepRebuilding)
                rebuild = true;
        }


    }
}