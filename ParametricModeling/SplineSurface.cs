using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineSurface : MonoBehaviour {

    public Transform[] LineA;
    public Transform[] LineB;
    public bool liveUpdate = false;
    public int uDivs, vDivs;

    void Start () {
        GetComponent<MeshFilter>().mesh = Grid.Generate(uDivs, vDivs, BezierSurface);
	}
	
	void Update () {
        if (liveUpdate)
        {
            Destroy(GetComponent<MeshFilter>().mesh);
            GetComponent<MeshFilter>().mesh = Grid.Generate(uDivs, vDivs, BezierSurface);
        }

	}

    Vector3 BezierSurface(float u, float v){
        return Vector3.Lerp(
        CatmullRomSpline.GetSplinePos(LineA, u),
        CatmullRomSpline.GetSplinePos(LineB, u),
            v);
    }
}
