using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR
{
    public class ArtSquareTree : ArtMakerTemplate
    {
        public GameObject branch;
        public GameObject fruit;

        public int amount = 10;
        float angle;
        float scaleBranches;

        public override void MakeArt()
        {
            angle = Random.value * 40;
            scaleBranches = Random.Range(.8f, .88f);
            GameObject tree = Instantiate(branch);
            tree.transform.localPosition = Vector3.down ;
            tree.transform.localScale = Vector3.one * .3f;
            tree.transform.SetParent(root.transform);
            Branch(tree, 0);
        }

        void Branch(GameObject g, int count){
            
            count++;

            if (count < amount)
            {
                GameObject b;

                if (Random.value > .2f)
                {
                    b = Instantiate(branch, g.transform);
                    b.transform.localPosition = new Vector3(0, 1, 0);
                    b.transform.localEulerAngles = new Vector3(0, 0, angle);
                    b.transform.localScale = Vector3.one * scaleBranches;
                    Branch(b, count);
                }

                if (Random.value > .2f)
                {
                    b = Instantiate(branch, g.transform);
                    b.transform.localPosition = new Vector3(0, 1, 0);
                    b.transform.localEulerAngles = new Vector3(0, 0, -angle);
                    b.transform.localScale = Vector3.one * scaleBranches;
                    Branch(b, count);
                }
            }
            else
            {
                GameObject b;

                b = Instantiate(fruit, g.transform);
                b.transform.localPosition = new Vector3(0, 1, 0);
                b.transform.localScale = Vector3.one ;

            }
        }
    }
}