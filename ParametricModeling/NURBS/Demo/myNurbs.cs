using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myNurbs : MonoBehaviour
{
    NURBS.CP[] points;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        points = new NURBS.CP[this.transform.childCount];
        for (int i = 0; i < this.transform.childCount; i++)
        {
            points[i] = new NURBS.CP(this.transform.GetChild(i).position, 1);
        }
        for (int i = 0; i < 100; i++)
        {
            Vector3 A = NURBS.Spline.GetCurve(points,(float)i / 100f);
            Vector3 B = NURBS.Spline.GetCurve(points,((float)i + 1f) / 100f);
            Debug.DrawLine(A, B, Color.white);
        }
    }
}
