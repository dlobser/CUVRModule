using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR
{
    public class ArtMakerLissajous : ArtMakerTemplate
    {

        public override void MakeArt()
        {
            GameObject g = new GameObject();
            g.transform.parent = root.transform;
            LineRenderer line = g.AddComponent<LineRenderer>();
            line.widthMultiplier = .01f;
            line.material = new Material(Shader.Find("Unlit/Color"));
            line.material.color = Color.black;
            line.positionCount = 1000;

            Vector3[] positions = new Vector3[1000];

            float randX = Random.value * .3f;
            float randY = Random.value * .3f;

            for (int i = 0; i < 1000; i++)
            {
                float fi = (float)i;
                float x = Mathf.Sin(fi * randX) * .5f;
                float y = Mathf.Cos(fi * randY) * .5f;
                positions[i] = new Vector3(x, y, 0);
            }

            line.SetPositions(positions);
        }
    }
}