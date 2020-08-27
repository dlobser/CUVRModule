using UnityEngine;
using System.Collections;

public static class CatmullRomSpline
{
   
    public static Vector3 GetSplinePos(Transform[] controlPointsList, float t)
    {
        int pos = (int)Mathf.Floor(t * (controlPointsList.Length - 1));

        //The 4 points we need to form a spline between p1 and p2
        Vector3 p0 = controlPointsList[ClampListPos(controlPointsList,pos - 1)].position;
        Vector3 p1 = controlPointsList[pos].position;
        Vector3 p2 = controlPointsList[ClampListPos(controlPointsList,pos + 1)].position;
        Vector3 p3 = controlPointsList[ClampListPos(controlPointsList,pos + 2)].position;

        return GetCatmullRomPosition(((t * (controlPointsList.Length - 1)) - pos), p0, p1, p2, p3);

    }

	public static Vector3 GetSplinePos(Vector3[] controlPointsList, float t)
	{
		int pos = (int)Mathf.Floor(t * (controlPointsList.Length - 1));

		//The 4 points we need to form a spline between p1 and p2
		Vector3 p0 = controlPointsList[ClampListPos(controlPointsList,pos - 1)];
		Vector3 p1 = controlPointsList[pos];
		Vector3 p2 = controlPointsList[ClampListPos(controlPointsList,pos + 1)];
		Vector3 p3 = controlPointsList[ClampListPos(controlPointsList,pos + 2)];

		return GetCatmullRomPosition(((t * (controlPointsList.Length - 1)) - pos), p0, p1, p2, p3);

	}

    //Clamp the list positions to allow looping
    static int ClampListPos(Transform[] controlPointsList, int pos)
    {
        if (pos < 0)
        {
			pos = 0;//controlPointsList.Length - 1;
        }

        if (pos > controlPointsList.Length)
        {
            pos = 1;
        }
        else if (pos > controlPointsList.Length - 1)
        {
			pos = controlPointsList.Length - 1;
        }

        return pos;
    }

	//Clamp the list positions to allow looping
	static int ClampListPos(Vector3[] controlPointsList, int pos)
	{
		if (pos < 0)
		{
			pos = 0;//controlPointsList.Length - 1;
		}

		if (pos > controlPointsList.Length)
		{
			pos = 1;
		}
		else if (pos > controlPointsList.Length - 1)
		{
			pos = controlPointsList.Length - 1;
		}

		return pos;
	}

    //Returns a position between 4 Vector3 with Catmull-Rom spline algorithm
    //http://www.iquilezles.org/www/articles/minispline/minispline.htm
    static Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        //The coefficients of the cubic polynomial (except the 0.5f * which I added later for performance)
        Vector3 a = 2f * p1;
        Vector3 b = p2 - p0;
        Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
        Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

        //The cubic polynomial: a + b * t + c * t^2 + d * t^3
        Vector3 pos = 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));

        return pos;
    }
}