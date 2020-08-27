using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupTubeRenderer : MonoBehaviour {

    TubeRenderer tube;
    public Transform[] tubePoints;
    public int detail;
    public float width;

	void OnEnable () {
        tube = GetComponent<TubeRenderer>();
        tube.vertices = new TubeRenderer.TubeVertex[detail];
        for (int i = 0; i < tube.vertices.Length; i++)
        {
            tube.vertices[i] = new TubeRenderer.TubeVertex(Vector3.zero, 1, Color.white);
        }
    }

    void Update () {
		for (int i = 0; i < detail; i++)
        {
            if (tube.vertices != null)
            {
                if (tube.vertices[i] != null)
                {
                    tube.vertices[i].point = CatmullRomSpline.GetSplinePos(tubePoints, (float)i / detail);
                    tube.vertices[i].radius = width;
                }
            }
        }
	}
}
