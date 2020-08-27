using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kmty.NURBS;

public class mySpline : MonoBehaviour
{
    public int order = 2;
    Spline spline;
    CP[] points;
    public float t;
    public GameObject tracer;

    void Start()
    {
        points = new CP[this.transform.childCount];
        for (int i = 0; i < this.transform.childCount; i++)
        {
            points[i] = new CP(this.transform.GetChild(i).position, 1);
        }
        spline = new Spline(points, order);
    }

    void Update()
    {
        points = new CP[this.transform.childCount];
        for (int i = 0; i < this.transform.childCount; i++)
        {
            points[i] = new CP(this.transform.GetChild(i).position, 1);
        }
        spline = new Spline(points, order);
        for (int i = 0; i < 100; i++)
        {
            Vector3 A = spline.GetCurve((float)i / 100f);
            Vector3 B = spline.GetCurve(((float)i + 1f) / 100f);
            Debug.DrawLine(A, B, Color.white);
        }
        tracer.transform.position = spline.GetCurve(t);
    }
}
