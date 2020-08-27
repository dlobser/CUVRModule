using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR
{
    public class ArtSquareCirclePacking : ArtMakerTemplate
    {
        public float largeSize = 2;
        public float smallSize = 1;
        public int num = 100;
        public float worldSize = 10;

        public override void MakeArt()
        {
            List<Vector3> circles = CirclePacker.PackCircles(smallSize, largeSize, num, worldSize);
            for (int i = 0; i < circles.Count; i++)
            {
                GameObject game = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                game.transform.parent = root.transform;
                game.transform.localPosition = new Vector3(circles[i].x, circles[i].y, 0);
                game.transform.localScale = new Vector3(circles[i].z, circles[i].z, circles[i].z);
            }
        }
    }
}