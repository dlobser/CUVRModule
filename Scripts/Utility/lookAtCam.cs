using UnityEngine;
using System.Collections;

public class lookAtCam : MonoBehaviour {

	public bool constrainY = false;
	public float addRotation = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Camera.main) {
			transform.LookAt (Camera.main.transform.position,Camera.main.transform.up);
			if (constrainY)
				transform.localEulerAngles = Vector3.Scale (transform.localEulerAngles, new Vector3 (0, 1, 0));
			transform.Rotate (0, addRotation, 0);
		}
	}
}
