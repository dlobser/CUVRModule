using UnityEngine;
using System;

public class Screenshot : MonoBehaviour
{
    public string screenshotName = "screenshot";
    public int superSize = 1;
    public bool takeSnapshot;

	private void Update()
	{
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S)){
            takeSnapshot = true;
        }

        if(takeSnapshot){
            ScreenCapture.CaptureScreenshot(screenshotName + DateTime.Now.ToString("_MM_dd_HH_mm_ss") + ".png", superSize);
            print("saved: " + screenshotName + DateTime.Now.ToString("_MM_dd_HH_mm_ss") + ".png");
            takeSnapshot = false;
        }
	}
}
