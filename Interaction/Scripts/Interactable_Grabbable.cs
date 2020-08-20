using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR{
    public class Interactable_Grabbable : Interactable
    {
        bool grab;
        bool prevGrab;
        Transform oldParent;
        Vector3 prevPos;
        public float moveTowardGrabberSpeed = 0;
        public float stopGrabbingDistance = .25f;

    	public override void HandleStart()
    	{
    		base.HandleStart();
            oldParent = this.transform.parent;
    	}

    	public override void HandleUpdate()
    	{
    		base.HandleUpdate();

            if (debug)
                Debug.Log("grabbed: " + grab + " , " + prevGrab);

            if (gaze != null)
            {
                if (!grab && prevGrab)
                {
                    if (this.GetComponent<Rigidbody>() != null)
                    {
                        this.GetComponent<Rigidbody>().isKinematic = false;
                        this.GetComponent<Rigidbody>().AddForce((this.transform.position - prevPos)/Time.deltaTime,ForceMode.VelocityChange);
                    }
                    this.transform.parent = oldParent;
                }
                prevGrab = grab;

                if (gaze.button.click < .5f && grab)
                {
                    grab = false;
                }
            }
            prevPos = this.transform.position;
    	}

    	public override void HandleHover()
        {
            base.HandleHover();
            float distance = Vector3.Distance(this.transform.position, gaze.transform.position);
            if (gaze.button.click > .5f && distance < minDistance)
            {
                if (debug)
                    Debug.Log("clicked");
                this.transform.parent = gaze.transform;
                if(distance>stopGrabbingDistance)
                    this.transform.position = Vector3.Lerp(this.transform.position, gaze.transform.position, moveTowardGrabberSpeed * Time.deltaTime);
                if (this.GetComponent<Rigidbody>() != null)
                    this.GetComponent<Rigidbody>().isKinematic = true;
                grab = true;
            }
        }
    }
}