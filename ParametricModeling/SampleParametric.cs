using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleParametric : MonoBehaviour {

    public int uDivs = 10;
    public int vDivs = 10;

    [Tooltip("0:plane,1:sphere,2:torus,3:noisySphere,4:wavyPath")]
    public int whichFunction;

    public bool rebuild;

	void Start () {

        Build();
	}

    void Build(){
        switch (whichFunction)
        {
            case 0:
                GetComponent<MeshFilter>().mesh = Grid.Generate(uDivs, vDivs, Plane);
                break;
            case 1:
                GetComponent<MeshFilter>().mesh = Grid.Generate(uDivs, vDivs, Sphere);
                break;
            case 2:
                GetComponent<MeshFilter>().mesh = Grid.Generate(uDivs, vDivs, Torus);
                break;
            case 3:
                GetComponent<MeshFilter>().mesh = Grid.Generate(uDivs, vDivs, NoisySphere);
                break;
            case 4:
                GetComponent<MeshFilter>().mesh = Grid.Generate(uDivs, vDivs, WavyPath);
                break;
            default: break;
        }
        GetComponent<MeshFilter>().mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh.RecalculateTangents();
    }
	private void Update()
	{
        if (rebuild)
        {
            Destroy(GetComponent<MeshFilter>().mesh);
            Build();
            rebuild = false;
        }
	}

	Vector3 Plane(float u, float v){
        return new Vector3(u, v, 0);
    }

    Vector3 Sphere(float u, float v)
    {
        float X = Mathf.Sin(u * Mathf.PI * 2) * Mathf.Sin(v * Mathf.PI);
        float Y = Mathf.Cos(v * Mathf.PI);
        float Z = Mathf.Cos(u * Mathf.PI * 2) * Mathf.Sin(v * Mathf.PI);
        return new Vector3(X,Y,Z);
    }

    Vector3 Torus(float u, float v){

        float X = (2 + Mathf.Cos(u * Mathf.PI * 2)) * Mathf.Cos(v * Mathf.PI * 2);
        float Y = (2 + Mathf.Cos(u * Mathf.PI * 2)) * Mathf.Sin(v * Mathf.PI * 2);
        float Z = 1 * Mathf.Sin(u * Mathf.PI * 2);
        return new Vector3(X, Y, Z);
    }

    Vector3 NoisySphere(float u, float v)
    {
        float X = Mathf.Sin(u * Mathf.PI * 2) * Mathf.Sin(v * Mathf.PI);
        float Y = Mathf.Cos(v * Mathf.PI);
        float Z = Mathf.Cos(u * Mathf.PI * 2) * Mathf.Sin(v * Mathf.PI);
        return new Vector3(X+Mathf.PerlinNoise(Y*10,Z)* .1f, Y+Mathf.PerlinNoise(X*10,Z)* .1f, Z+Mathf.PerlinNoise(X*10,Y)*.1f);
    }

    Vector3 WavyPath(float u, float v)
    {
        float X = u;
        float Y = 0;
        float Z = Mathf.Cos(u * Mathf.PI * 4)*.1f + v*.1f;
        return new Vector3(X,Y,Z);
    }
}
