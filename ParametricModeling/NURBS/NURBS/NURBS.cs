using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NURBS
{
    public struct CP
    {
        public Vector3 pos;
        public float weight;
        public CP(Vector3 p, float w) { pos = p; weight = w; }
    }

    public static class Spline : object
    {


        //public Spline(CP[] originalCps, int order)
        //{
        //    var ol = originalCps.Length;
        //    var nl = ol + 2 * order;
        //    cps = new CP[nl];
        //    System.Array.Copy(originalCps, 0, cps, order, ol);
        //    for (int i = 0; i < order; i++)
        //    {
        //        cps[i] = originalCps[0];
        //        cps[nl - i - 1] = originalCps[ol - 1];
        //    }
        //    this.order = order;
        //}

        public static CP[] SetupSpline(CP[] originalCps, int order)
        {
            var ol = originalCps.Length;
            var nl = ol + 2 * order;
            CP[] cps = new CP[nl];
            System.Array.Copy(originalCps, 0, cps, order, ol);
            for (int i = 0; i < order; i++)
            {
                cps[i] = originalCps[0];
                cps[nl - i - 1] = originalCps[ol - 1];
            }
            return cps;
        }
        public static Vector3 GetCurve(CP[] cps, float t, int order = 3)
        {
            cps = SetupSpline(cps, order);
            t = Mathf.Clamp01(t);
            var frac = Vector3.zero;
            var deno = 0f;
            for (int i = 0; i < cps.Length; i++)
            {
                var bf = BasisFunc(cps, order, i, order, t);
                var cp = cps[i];
                frac += cp.pos * bf * cp.weight;
                deno += bf * cp.weight;
            }
            return frac / deno;
        }

        //public static void UpdateCP(int i, CP cp)
        //{
        //    var cl = cps.Length;
        //    var ol = cl - order * 2;
        //    if (i == 0) for (int j = 0; j <= order; j++) cps[j] = cp;
        //    else if (i == ol - 1) for (int j = 0; j <= order; j++) cps[cl - j - 1] = cp;
        //    else cps[i + order] = cp;
        //}

        public static float BasisFunc(CP[] cps, int order, int j, int k, float t)
        {
            if (k == 0)
            {
                return (t >= KnotVector(j, cps, order) && t < KnotVector(j + 1, cps, order)) ? 1 : 0;
            }
            else
            {
                var d1 = KnotVector(j + k, cps, order) - KnotVector(j, cps, order);
                var d2 = KnotVector(j + k + 1, cps, order) - KnotVector(j + 1, cps, order);
                var c1 = d1 != 0 ? (t - KnotVector(j, cps, order)) / d1 : 0;
                var c2 = d2 != 0 ? (KnotVector(j + k + 1, cps, order) - t) / d2 : 0;
                return c1 * BasisFunc(cps, order, j, k - 1, t) + c2 * BasisFunc(cps, order, j + 1, k - 1, t);
            }
        }

        public static float KnotVector(int j, CP[] cps, int order)
        {
            var m = cps.Length + order + 1;
            if (j < order + 1) return 0;
            if (j > m - (order + 1)) return 1;
            return Mathf.Max(j - order, 0f) / (m - 2 * (order + 1));
        }

    }
}