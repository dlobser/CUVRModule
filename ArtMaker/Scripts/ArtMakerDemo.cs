using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR
{
    public class ArtMakerDemo : ArtMakerTemplate
    {
        public GameObject thing;
        public int amount = 10;

        public override void MakeArt()
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject g = Instantiate(thing, root.transform);
                g.transform.localPosition = Random.insideUnitSphere * .5f;
                g.transform.localEulerAngles = Random.insideUnitSphere * 360;
                g.transform.localScale = Vector3.one * .1f;
            }

        }
    }
}