using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFaces : MonoBehaviour {



	// Use this for initialization
	void Start () {

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        Vector3[] vertices = new Vector3[300];
        int[] triangles = new int[900];

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = Random.insideUnitSphere;
           
        }
        for (int i = 0; i < triangles.Length; i++)
        {
            triangles[i] = (int)Random.Range(0, 900);
        }
    }

    // Update is called once per frame
	void Update () {
		
	}
}
