
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CirclePacker
{

    public static float largeSize = .1f;
    public static float smallSize = .01f;
    public static int num = 250;
    public static float worldSize = .5f;
    static float[,] circmat;
    public static List<Vector3> positionAndRadius;

    /// <summary>
    /// Returns a list with x,y,radius of packed circls
    /// </summary>
    /// <returns>The circles.</returns>
    public static List<Vector3> PackCircles()
    {
        positionAndRadius = new List<Vector3>();
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
    public static List<Vector3> PackCircles(float small, float large, int amt, float size)
    {
        smallSize = small;
        largeSize = large;
        num = amt;
        worldSize = size;
        positionAndRadius = new List<Vector3>();
        circmat = initializeCoords(num);
        FindAllRadii(circmat, 2);
        return positionAndRadius;
    }

    static float[,] initializeCoords(int numell)
    {
        float[,] outmat = new float[numell, 3];
        for (int i = 0; i < numell; i++)
        {
            outmat[i, 0] = Random.Range(-worldSize, worldSize);
            outmat[i, 1] = Random.Range(-worldSize, worldSize);
            outmat[i, 2] = 0;
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
            r = Mathf.Sqrt(Mathf.Pow(cmat[i, 0] - cmat[indx, 0], 2)
                    + Mathf.Pow(cmat[i, 1] - cmat[indx, 1], 2))
                        - cmat[i, 2];
            if (cmat[i, 2].Equals(0)) r = largeSize;
            distArr[i] = r;
        }

        radius = Mathf.Min(Mathf.Min(distArr), largeSize);

        if (radius > smallSize)
        {
            cmat[indx, 2] = radius;
            radius = radius * 2;
            positionAndRadius.Add(new Vector3(cmat[indx, 0], cmat[indx, 1], radius));
            return cmat;
        }

        cmat[indx, 0] = Random.Range(-worldSize, worldSize);
        cmat[indx, 1] = Random.Range(-worldSize, worldSize);

        return FindNextRadius(cmat, indx);

    }


}

