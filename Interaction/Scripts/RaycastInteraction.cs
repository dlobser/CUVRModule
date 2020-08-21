using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR{

    public class RaycastInteraction : MonoBehaviour {
        
        [Tooltip("Used to link specific raycasters with interactables")]
        public string type;

        public Vector3 hitPosition { get; set; }
        public Vector3 hitNormal{ get; set; }
        public GameObject hitObject{ get; set; }

        public bool useMouse;

    	public delegate void MouseHasHit();
    	public static event MouseHasHit mouseHasHit;
        [Tooltip("Turn this off to only raycast when a button is clicked")]
        public bool alwaysActive = true;
        [Tooltip("Drag in a button component, or leave this slot empty")]
        public Button button;
        float click;
        int layerMask;
        [Tooltip("Raycast to layers, or leave blank for all")]
        public string[] layers;
        [Tooltip("Only raycast the first object")]
        public bool firstRaycastOnly = true;

    	void Start() {
            if (button == null)
                alwaysActive = true;
            if(layers.Length>0)
                layerMask = LayerMask.GetMask(layers);
        }


        void Update() {

            if (alwaysActive || button != null && button.click > .5f)
            {
                RaycastHit[] hits = new RaycastHit[0];
                RaycastHit hit = new RaycastHit();

                if (firstRaycastOnly)
                {
                    bool didHit;
                    if (useMouse)
                    {
                        if (layers.Length > 0)
                            didHit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1e6f, layerMask);
                        else
                            didHit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
                    }
                    else
                    {
                        if (layers.Length > 0)
                            didHit = Physics.Raycast(new Ray(this.transform.position, this.transform.forward), out hit, 1e6f, layerMask);
                        else
                            didHit = Physics.Raycast(new Ray(this.transform.position, this.transform.forward), out hit);
                    }
                    if (didHit)
                    {
                        hits = new RaycastHit[1];
                        hits[0] = hit;
                    }
                }
                else
                {
                    if (useMouse)
                    {
                        if (layers.Length > 0)
                            hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 1e6f, layerMask);
                        else
                            hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
                    }
                    else
                    {
                        if (layers.Length > 0)
                            hits = Physics.RaycastAll(new Ray(this.transform.position, this.transform.forward), 1e6f, layerMask);
                        else
                            hits = Physics.RaycastAll(new Ray(this.transform.position, this.transform.forward));
                    }
                }
                if (hits.Length>0)
                {
                    hitPosition = hits[0].point;
                    hitNormal = hits[0].normal;
                    hitObject = hits[0].collider.gameObject;

                    click = 0;
                    if (button != null)
                        click = button.click;
                        
                    for(int i = 0; i < (firstRaycastOnly ? 1 : hits.Length); i ++)
                    {
                        print(hits[i].transform.gameObject.name);
                        if (hits[i].transform.gameObject.GetComponent<Interactable>() != null)
                        {
                            Interactable[] interactables = hits[i].transform.gameObject.GetComponents<Interactable>();
                            foreach (Interactable t in interactables)
                            {
                                t.Ping(this, click, type);
                            }
                            if (hits[i].transform.gameObject.GetComponent<Interactable_Blocker>() != null)
                                break;
                        }

                    }
                }
                else
                {
                    hitPosition = Vector3.zero;
                    hitNormal = Vector3.zero;
                    hitObject = null;
                }
            }

    	}
    }
}