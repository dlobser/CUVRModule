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
        [Tooltip("turn this off to only raycast when a button is clicked")]
        public bool alwaysActive = true;
        [Tooltip("Drag in a button component, or leave this slot empty")]
        public Button button;
        float click;
        int layerMask;
        [Tooltip("raycast to layers, or leave blank for all")]
        public string[] layers;

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

                if (useMouse)
                {
                    if(layers.Length>0)
                        hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition),1e6f,layerMask);
                    else
                        hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
                }
                else
                {
                    if (layers.Length > 0)
                        hits = Physics.RaycastAll(new Ray(this.transform.position, this.transform.forward),1e6f,layerMask);
                    else
                        hits = Physics.RaycastAll(new Ray(this.transform.position, this.transform.forward));
                }

                if (hits.Length>0)
                {
                    hitPosition = hits[0].point;
                    hitNormal = hits[0].normal;
                    hitObject = hits[0].collider.gameObject;

                    click = 0;
                    if (button != null)
                        click = button.click;
                        
                    foreach(RaycastHit h in hits)
                    {
                        if (h.transform.gameObject.GetComponent<Interactable>() != null)
                        {
                            Interactable[] interactables = h.transform.gameObject.GetComponents<Interactable>();
                            foreach (Interactable i in interactables)
                            {
                                i.Ping(this, click, type);
                            }
                            if (h.transform.gameObject.GetComponent<Interactable_Blocker>() != null)
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