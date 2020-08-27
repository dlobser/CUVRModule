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
    public class ArtSquareGrid : ArtMakerTemplate
    {
        public GameObject[] things;
        public int amount = 10;

        public override void MakeArt()
        {
            int amt = Random.Range(1, amount);
            for (int i = 0; i < amt; i++)
            {
                int index = Random.Range(0, things.Length);
                for (int j = 0; j < amt; j++)
                {
                    GameObject g = Instantiate(things[index]);
                    g.transform.parent = root.transform;
                    float size = ((float)1 / (float)amt);
                    g.transform.localScale = new Vector3(size, size, size);
                    g.transform.position = new Vector3(
                        (float)j/(float)amt - .5f + size*.5f, 
                        (float)i/(float)amt - .5f + size*.5f, 
                        0);
                    Color color = Random.ColorHSV();
                    g.GetComponent<MeshRenderer>().material.color = color;
                }
            }
            //int amt = Random.Range(1, amount);
            //for (int i = 0; i < amt; i++)
            //{
            //    int index = Random.Range(0, things.Length);
            //    for (int j = 0; j < amt; j++)
            //    {
            //        GameObject g = Instantiate(things[index]);
            //        g.transform.parent = root.transform;
            //        float size = ((float)1 / (float)amt);
            //        g.transform.localScale = new Vector3(size*.1f, size*Random.Range(1,15), size);
            //        g.transform.localEulerAngles = new Vector3(0, 0, Random.Range(0,8)*45);
            //        g.transform.position = new Vector3(
            //            (float)j / (float)amt - .5f + size * .5f,
            //            (float)i / (float)amt - .5f + size * .5f,
            //            0);
            //        Color color = Random.ColorHSV();
            //        g.GetComponent<MeshRenderer>().material.color = color;
            //    }
            //}
        }
    }
}