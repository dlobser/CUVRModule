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

        [Tooltip("Turn this off to only raycast when a button is clicked")]
        public bool alwaysActive = true;
        [Tooltip("Drag in a button component, or leave this slot empty")]
        public Button button;
        float click;
        int layerMask;
        [Tooltip("Raycast to layers, or leave blank for all")]
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
                RaycastHit hit = new RaycastHit();
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
                    hitPosition = hit.point;
                    hitNormal = hit.normal;
                    hitObject = hit.collider.gameObject;

                    click = 0;
                    if (button != null)
                        click = button.click;
                        

                    if (hit.transform.gameObject.GetComponent<Interactable>() != null)
                    {
                        Interactable[] interactables = hit.transform.gameObject.GetComponents<Interactable>();
                        foreach (Interactable t in interactables)
                        {
                            t.Ping(this, click, type);
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