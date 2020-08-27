using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR
{
    public class ArtMakerDragonCurve : ArtMakerTemplate
    {
        public Transform limb;
        public int recursions;

        public override void MakeArt()
        {
            GameObject dragon = Instantiate(limb.gameObject);
            dragon.transform.position = Vector3.zero;
            Dragonize(dragon.transform, 0, 0);
        }

        void Dragonize(Transform t, int count, int which){
            if (count < recursions)
            {
                count++;
                print(t.childCount);
                Transform[] things = new Transform[2];
                for (int i = 0; i < t.childCount; i++)
                {
                    Transform t1 = t.GetChild(i);
                    Transform tb = Instantiate(limb);
                    things[i] = tb;
                    print(count + " , " + i);
                    tb.transform.parent = t1.transform;
                    if (which==1)
                    {
                        if (i == 1)
                            tb.transform.localEulerAngles = new Vector3(0, 0, 45);
                        else
                            tb.transform.localEulerAngles = new Vector3(0, 180, 45);
                    }
                    else
                    {
                        if (i == 0)
                            tb.transform.localEulerAngles = new Vector3(0, 0, 45);
                        else
                            tb.transform.localEulerAngles = new Vector3(0, 180, 45);
                    }

                    tb.transform.localPosition = Vector3.zero;
                    float s = 0.707106f;
                    tb.transform.localScale = new Vector3(s, s, s);
                    tb.transform.parent = root.transform;
                    Dragonize(tb, count, i);
                }
                Destroy(t.gameObject);
                //foreach(Transform tt in things){
                //    Dragonize(tt,count);
                //}


            }

        }

    }
}