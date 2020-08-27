using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script randomly selects items from an array
 * and scatters them randomly
 * It parents the items to the root object
 * so that they will be destroyed when 'rebuild' is true
 */

namespace CUVR
{
    public class ArtSquareSimple : ArtMakerTemplate
    {
        public GameObject[] objects;
        public int amount = 10;

        public override void MakeArt()
        {
            int amt = Random.Range(2, amount);
            for (int i = 0; i < amt; i++)
            {
                GameObject g = Instantiate(objects[Random.Range(0, objects.Length)]);
                g.transform.parent = root.transform;
                g.GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
                float s = Random.value * .25f;
                g.transform.localScale = new Vector3(s, s, s);
                g.transform.position = Random.insideUnitSphere*.5f;
            }
        }
    }
}