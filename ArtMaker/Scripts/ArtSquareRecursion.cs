using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR
{
    public class ArtSquareRecursion : ArtMakerTemplate
    {
        public int amount = 10;

        [Tooltip("Put 2 objects here")]
        public GameObject[] objects;

        float randomHue;
        float complementaryHue;

        public override void MakeArt()
        {

            GameObject cube = Instantiate(objects[0]);
            cube.transform.SetParent(root.transform);
            Recurse(0, cube);
            randomHue = Random.value;
            complementaryHue = (randomHue + .5f) % 1;
            cube.transform.localScale = new Vector3(1, 1, .1f);
            cube.GetComponent<MeshRenderer>().material.color = 
                Color.HSVToRGB(Random.Range(randomHue, randomHue + .1f), Random.Range(.5f, .7f), .95f);

        }

        void Recurse(int count, GameObject game){
            
            count++;

            if (count < amount)
            {
                for (int i = 0; i < 4; i++)
                {
                    GameObject cube;

                    if (Random.value > .5f)
                        cube = Instantiate(objects[0]);
                    else
                        cube = Instantiate(objects[1]);
                    
                    if (Random.value > .5f)
                        cube.GetComponent<MeshRenderer>().material.color = 
                            Color.HSVToRGB(Random.Range(randomHue,randomHue+.1f), Random.Range(.5f, 1f), .6f);
                    else
                        cube.GetComponent<MeshRenderer>().material.color = 
                            Color.HSVToRGB(Random.Range(complementaryHue,complementaryHue+.1f), Random.Range(.5f, 1f), 1f);

                    cube.transform.SetParent(game.transform);
                    Vector3 pos = Vector3.zero;
                    Vector3 scale = Vector3.one * .5f;

                    int which = Random.Range(0, 4);

                    if (i == 0)
                    {
                        pos = new Vector3(.25f, .25f, -.5f);
                    }
                    else if (i == 1)
                    {
                        pos = new Vector3(.25f, -.25f, -.5f);
                    }
                    else if (i == 2)
                    {
                        pos = new Vector3(-.25f, .25f, -.5f);
                    }
                    else if (i == 3)
                    {
                        pos = new Vector3(-.25f, -.25f, -.5f);
                    }

                    cube.transform.localPosition = pos;
                    cube.transform.localScale = scale;
                    if (Random.value > .5f)
                        Recurse(count, cube);
                }

            }
        }

    }
}