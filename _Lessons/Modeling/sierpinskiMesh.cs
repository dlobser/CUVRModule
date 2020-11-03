using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sierpinskiMesh : MonoBehaviour
{

    Mesh mesh;
    Mesh origMesh;
    public int recurseAmount = 3;
    List<Vector3[]> tris;
    public MeshFilter otherMesh;

    void Start()
    {
        mesh = new Mesh();
        origMesh = GetComponent<MeshFilter>().mesh;
        print(origMesh.vertices.Length);
        tris = new List<Vector3[]>();
        for (int i = 0; i < origMesh.triangles.Length; i+=3)
        {
            Vector3[] vec = new Vector3[3];
            vec[0] = origMesh.vertices[origMesh.triangles[i]];
            vec[1] = origMesh.vertices[origMesh.triangles[i+1]];
            vec[2] = origMesh.vertices[origMesh.triangles[i+2]];
            Recurse(0, vec);
        }
        int amt = (int)Mathf.Pow(origMesh.vertices.Length, recurseAmount);
        print(amt);
        Vector3[] v = new Vector3[tris.Count*3];
        int[] triangles = new int[tris.Count*3];
        print(tris.Count);
        int c = 0;
        for (int i = 0; i < tris.Count; i++)
        {
            if (tris[i][3].Equals(Vector3.one))
            {
                v[c++] = tris[i][2];
                triangles[c] = c;
                v[c++] = tris[i][1];
                triangles[c] = c;
                v[c++] = tris[i][0];
                triangles[c] = c;
            }
        }

        mesh.vertices = v;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        otherMesh.mesh = mesh;
    }

    void Recurse(int count, Vector3[] V)
    {
        Vector3 A = V[0];
        Vector3 B = V[1];
        Vector3 C = V[2];

        Vector3[] TA = new Vector3[4];
        TA[2] = Vector3.Lerp(A, B, .5f);
        TA[1] = Vector3.Lerp(A, B, 1);
        TA[0] = Vector3.Lerp(B, C, .5f);
        //used to determine if this triangle was generated on the last iteration
        //so the final mesh doesn't have overlapping faces
        TA[3] = count==recurseAmount ? Vector3.one : Vector3.zero;
        tris.Add(TA);

        Vector3[] TB = new Vector3[4];
        TB[2] = Vector3.Lerp(B, C, .5f);
        TB[1] = Vector3.Lerp(B, C, 1);
        TB[0] = Vector3.Lerp(C, A, .5f);
        TB[3] = count == recurseAmount ? Vector3.one : Vector3.zero;
        tris.Add(TB);

        Vector3[] TC = new Vector3[4];
        TC[2] = Vector3.Lerp(C, A, .5f);
        TC[1] = Vector3.Lerp(C, A, 1);
        TC[0] = Vector3.Lerp(A, B, .5f);
        TC[3] = count == recurseAmount ? Vector3.one : Vector3.zero;
        tris.Add(TC);

        //use this if you want to fill in the central triangle
        //Vector3[] TD = new Vector3[4];
        //TD[2] = Vector3.Lerp(C, A, .5f);
        //TD[1] = Vector3.Lerp(A, B, .5f);
        //TD[0] = Vector3.Lerp(B, C, .5f);
        //TD[3] = count == recurseAmount ? Vector3.one : Vector3.zero;
        //tris.Add(TC);
        //print(TA);

        if (count < recurseAmount)
        {
            count++;
            Recurse(count, TA);
            Recurse(count, TB);
            Recurse(count, TC);
            //Recurse(count, TD);
        }
    }

}
