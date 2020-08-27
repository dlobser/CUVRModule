using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR
{
    public class ArtSquareSpherePacking : ArtMakerTemplate
    {
        public GameObject[] ball;

        public Vector2 largeSize = Vector2.one*.2f;
        public Vector2 smallSize = Vector2.one*.02f;
        public Vector2 num = Vector2.one*100;
        public float worldSize = 10;

        public override void MakeArt()
        {
            int amount = (int)Random.Range(num.x, num.y);
            float big = Random.Range(largeSize.x, largeSize.y);
            float small = Random.Range(smallSize.x, smallSize.y);

            List<Vector4> circles = SpherePacker.PackCircles(small, big, amount, worldSize);
            for (int i = 0; i < circles.Count; i++)
            {
                GameObject game = Instantiate(ball[Random.Range(0,ball.Length)]);
                game.transform.parent = root.transform;
                game.transform.localPosition = new Vector3(circles[i].x, circles[i].y, circles[i].z);
                game.transform.localScale = new Vector3(circles[i].w, circles[i].w, circles[i].w);
                game.transform.localEulerAngles = Random.insideUnitSphere * 360f;

            }
        }

    }
}