using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR
{
    public class ArtSquareConnectedDots : ArtMakerTemplate
    {
        public GameObject line;
        public int amount = 10;
        GameObject[] lines;
        Vector3[] points;

        public override void MakeArt()
        {
            int amt = Random.Range(3, amount);

            points = new Vector3[amt];

            for (int i = 0; i < amt; i++)
            {
                points[i] = Random.insideUnitSphere*.5f;
            }

            for (int i = 0; i < amt; i++)
            {
                for (int j = 0; j < amt; j++)
                {
                    if(i!=j){
                        GameObject thisLine = Instantiate(line);
                        thisLine.transform.position = Vector3.Lerp(points[i], points[j], .5f);
                        float size = Vector3.Distance(points[i],points[j]);
                        thisLine.transform.localScale = new Vector3(.005f, .005f, size );
                        thisLine.transform.LookAt(points[i]);
                        thisLine.transform.parent = root.transform;
                    }
                }
            }
        }
    }
}