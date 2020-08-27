using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR
{
    public class ArtSquareSpiral : ArtMakerTemplate
    {
        public int amount = 10;
        public float widthMultiplier;
        public Material material;
        public float size = .1f;
        public float detail = .4f;

        public override void MakeArt()
        {
            GameObject g = new GameObject();
            g.transform.SetParent(root.transform);
            LineRenderer line = g.AddComponent<LineRenderer>();
            line.positionCount = amount;
            line.material = material;
            line.widthMultiplier = widthMultiplier;

            detail = Random.Range(.05f, .2f);
            size = Random.Range(.001f, .004f);
            float wave = Random.Range(1f, 2f);
            float mult = Random.Range(.05f, .15f);

            for (int i = 0; i < amount; i++)
            {
                float m = (Mathf.Sin(i * detail * 3 * wave) * mult) + 1;
                float x = Mathf.Sin(i * detail) * (i * size * m);
                float y = Mathf.Cos(i * detail) * (i * size * m);

                line.SetPosition(i, new Vector3(x, y, 0));
            }

        }
    }
}