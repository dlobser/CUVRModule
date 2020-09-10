using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR{

    public class LaserPointer : MonoBehaviour {

        public RaycastHit hit;
        [Tooltip("Not required: This object will appear where the laser hits")]
        public GameObject endPoint;
        [Tooltip("Not required: Add a button to only activate the laser when it's pressed")]
        public Button button;
        MeshRenderer[] renderers;

    	private void Start()
    	{
            renderers = GetComponentsInChildren<MeshRenderer>();
    	}

    	void Update() {
            if (button == null || button.click > .5f)
            {
                foreach (MeshRenderer m in renderers)
                {
                    m.enabled = true;
                }
                if (Physics.Raycast(transform.position, transform.forward, out hit))
                {
                    transform.localScale = new Vector3(1, 1, Vector3.Distance(this.transform.position, hit.point));
                    if (endPoint != null)
                    {
                        endPoint.transform.position = hit.point;
                        endPoint.SetActive(true);
                    }

                }
                else
                {
                    if (endPoint != null)
                        endPoint.SetActive(false);
                    transform.localScale = new Vector3(1, 1, 100);
                }
            }
            else{
                foreach(MeshRenderer m in renderers){
                    m.enabled = false;
                }
            }
        }
    }


}