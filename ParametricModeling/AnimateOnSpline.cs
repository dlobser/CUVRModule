using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class AnimateOnSpline : MonoBehaviour {

	public Transform controllers;
    Transform[] pos;
    public float t = 0;
	Vector3 prev;
	public float speed;

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        for (int i = 0; i < 29; i++)
        {
            Gizmos.DrawLine(CatmullRomSpline.GetSplinePos(pos, (float)i/30),
                            CatmullRomSpline.GetSplinePos(pos, ((float)i+1)/30));
        }

    }

	void Start () {
		pos = new Transform[controllers.childCount];
		for (int i = 0; i < pos.Length; i++) {
			pos [i] = controllers.GetChild (i);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (speed != 0) {
			t += Time.deltaTime * speed;
		}
        this.transform.position = CatmullRomSpline.GetSplinePos(pos, Mathf.Clamp(t%1,0,1));
		this.transform.LookAt (CatmullRomSpline.GetSplinePos(pos, Mathf.Clamp((t-.001f)%1,0,1)));
		prev = this.transform.position;
	}
}
