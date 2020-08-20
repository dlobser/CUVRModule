using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR{

public class Button : MonoBehaviour
{
    public bool useButton = true;
    public bool useAxis;
    public string buttonName = "Fire1";
    public float click;

    bool down;
    bool prevDown;
    public bool buttonDown { get; set; }
    public bool buttonUp { get; set; }

    void Update()
    {
        if (useButton)
            click = Input.GetButton(buttonName) ? 1 : 0;
        else if (useAxis)
            click = Input.GetAxis(buttonName);

        if(click>0 && !down){
            down = true;
        }
        else if(click.Equals(0)&&down){
            down = false;
        }

        Debug.Log(down + " , " + buttonDown);
        if (down && !prevDown)
            buttonDown = true;
        else if (!down && prevDown)
            buttonUp = true;
        else{
            buttonDown = false;
            buttonUp = false;
        }
        prevDown = down;
    }
}


}