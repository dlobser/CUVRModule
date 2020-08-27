
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpherePacker
{

    public static float largeSize = .1f;
    public static float smallSize = .01f;
    public static int num = 250;
    public static float worldSize = .5f;
    static float[,] circmat;
    public static List<Vector4> positionAndRadius;

    /// <summary>
    /// Returns a list with x,y,radius of packed circls
    /// </summary>
    /// <returns>The circles.</returns>
    public static List<Vector4> PackCircles()
    {
        positionAndRadius = new List<Vector4>();
        circmat = initializeCoords(num);
        FindAllRadii(circmat, 2);
        return positionAndRadius;
    }

    /// <summary>
    /// Returns a list with x,y,radius of packed circls
    /// </summary>
    /// <returns>The circles.</returns>
    /// <param name="small">smallest circle.</param>
    /// <param name="large">largest circle.</param>
    /// <param name="amt">Amount of circles.</param>
    /// <param name="size">World extents.</param>
    public static List<Vector4> PackCircles(float small, float large, int amt, float size)
    {
        smallSize = small;
        largeSize = large;
        num = amt;
        worldSize = size;
        positionAndRadius = new List<Vector4>();
        circmat = initializeCoords(num);
        FindAllRadii(circmat, 2);
        return positionAndRadius;
    }

    static float[,] initializeCoords(int numell)
    {
        float[,] outmat = new float[numell, 4];
        for (int i = 0; i < numell; i++)
        {
            Vector3 vector = Vector3.Normalize(Random.insideUnitSphere) * worldSize;
            outmat[i, 0] = vector.x;//Random.Range(-worldSize, worldSize);
            outmat[i, 1] = vector.y;//Random.Range(-worldSize, worldSize);
            outmat[i, 2] = vector.z;//Random.Range(-worldSize, worldSize);
            outmat[i, 3] = 0;
        }
        return outmat;
    }

    static bool FindAllRadii(float[,] cmat, int indx)
    {
        if (indx == num) return false;
        return FindAllRadii(FindNextRadius(cmat, indx), indx + 1);
    }

    static float[,] FindNextRadius(float[,] cmat, int indx)
    {
        float[] distArr = new float[num - 1];
        float radius, r;
        for (int i = 0; i < num - 1; i++)
        {
            r = Mathf.Sqrt(
                Mathf.Pow(cmat[i, 0] - cmat[indx, 0], 2) + 
                Mathf.Pow(cmat[i, 1] - cmat[indx, 1], 2) +
                Mathf.Pow(cmat[i, 2] - cmat[indx, 2], 2))
                        - cmat[i, 3];
            if (cmat[i, 3].Equals(0)) r = largeSize;
            distArr[i] = r;
        }

        radius = Mathf.Min(Mathf.Min(distArr), largeSize);

        if (radius > smallSize)
        {
            cmat[indx, 3] = radius;
            radius = radius * 2;
            positionAndRadius.Add(new Vector4(cmat[indx, 0], cmat[indx, 1], cmat[indx, 2], radius));
            return cmat;
        }

        Vector3 vector = Vector3.Normalize(Random.insideUnitSphere) * worldSize;
        cmat[indx, 0] = vector.x;
        cmat[indx, 1] = vector.y;
        cmat[indx, 2] = vector.z;
        return FindNextRadius(cmat, indx);

    }


}

