using UnityEngine;

public class lookAtAnotherObject : MonoBehaviour {

    public Transform target;
	
	void Update () {
		this.transform.LookAt (target);
	}
}
