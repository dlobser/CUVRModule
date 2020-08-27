using UnityEngine;
using System.Collections;

//It is common to create a class to contain all of your
//extension methods. This class must be static.
public static class ExtensionMethods
{
    //Even though they are used like normal methods, extension
    //methods must be declared static. Notice that the first
    //parameter has the 'this' keyword followed by a Transform
    //variable. This variable denotes which class the extension
    //method becomes a part of.
    public static void ResetTransformation(this Transform trans)
    {
        trans.localPosition = Vector3.zero;
        trans.localRotation = Quaternion.identity;
        trans.localScale = new Vector3(1, 1, 1);
    }

    public static void SetPositionX(this Transform trans, float x)
    {
        trans.position = new Vector3(x, trans.position.y, trans.position.y);
    }
    public static void SetPositionY(this Transform trans, float y)
    {
        trans.position = new Vector3(y, trans.position.y, trans.position.y);
    }
    public static void SetPositionZ(this Transform trans, float z)
    {
        trans.position = new Vector3(z, trans.position.y, trans.position.y);
    }

    public static void SetLocalPositionX(this Transform trans, float x)
    {
        trans.localPosition = new Vector3(x, trans.position.y, trans.position.y);
    }
    public static void SetLocalPositionY(this Transform trans, float y)
    {
        trans.localPosition = new Vector3(y, trans.position.y, trans.position.y);
    }
    public static void SetLocalPositionZ(this Transform trans, float z)
    {
        trans.localPosition = new Vector3(z, trans.position.y, trans.position.y);
    }
   
}