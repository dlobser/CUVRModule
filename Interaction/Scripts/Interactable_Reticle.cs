using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR { 
    public class Interactable_Reticle : Interactable
    {
        public GameObject reticle;
        GameObject thisReticle;
        public Interactable[] interactable;
        bool interactableTriggered = false;
        public bool deactivateOnTrigger = false;
        public bool swap;
        public bool swapper;
        public float returnSpeed = 1;

        public bool repositionReticle = false;

        public override void HandleHover()
        {
            base.HandleHover();

            if (thisReticle == null)
            {
                thisReticle = reticle;// Instantiate(reticle,this.transform);
            }

            if (!interactableTriggered && hoverCounter >= hoverTime)
            {
                if (interactable != null)
                    foreach (Interactable i in interactable)
                    {
                        if (!i.gameObject.activeInHierarchy)
                            i.gameObject.SetActive(true);
                        i.HandleTrigger();
                    }
                interactableTriggered = true;
            }

            if (hoverCounter < hoverTime)
            {
                if (repositionReticle)
                            thisReticle.transform.position = this.transform.position;
                thisReticle.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("_Percent", swap ? hoverCounter / hoverTime : 1 - (hoverCounter / hoverTime));
                if (deactivateOnTrigger)
                {
                    thisReticle.transform.GetChild(0).gameObject.SetActive(true);
                }
            }
            else
            {
                if (deactivateOnTrigger)
                    thisReticle.transform.GetChild(0).gameObject.SetActive(false);
            }
        }

        public override void HandleTrigger()
        {
            base.HandleTrigger();
            if (thisReticle == null){thisReticle = reticle;}
            if (interactable != null)
                foreach (Interactable i in interactable)
                {
                    if (!i.gameObject.activeInHierarchy)
                        i.gameObject.SetActive(true);
                    i.HandleTrigger();
                }
            interactableTriggered = true;
            if (deactivateOnTrigger)
            {
                thisReticle.transform.GetChild(0).gameObject.SetActive(false);
                Debug.Log("Hovering");
            }
        }

        public override void HandleExit()
        {
            base.HandleExit();
            if (thisReticle == null)
            {
                thisReticle = reticle;
            }
            hoverCounter = 0;
        }

        private void OnDisable()
        {
            hoverCounter = 0;
        }

        public override void HandleEnter()
        {
            base.HandleEnter();
            if (thisReticle == null)
            {
                thisReticle = reticle;
            }
            thisReticle.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("_Percent", 1);
            hoverCounter = 0;
            thisReticle.transform.GetChild(0).gameObject.SetActive(true);
        }

        public override void HandleWaiting(){
    		base.HandleWaiting();
            if (thisReticle != null)
            {
                if (hoverCounter >= 0)
                {
                    hoverCounter -= returnSpeed * Time.deltaTime;
                    thisReticle.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("_Percent", swap ? (hoverCounter / hoverTime) : (1 - (hoverCounter / hoverTime)));
                }
                else
                {   
                    interactableTriggered = false;
                }
            }
    	}
    }
}