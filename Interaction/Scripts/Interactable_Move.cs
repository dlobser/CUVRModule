using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR{

    public class Interactable_Move : Interactable
    {
        //navigate to the raycast position
        public GameObject target;


        public override void HandleHover()
        {
            if (gaze.button.buttonUp)
            {
                HandleTrigger();
            }
        }

        public override void HandleTrigger()
        {
            base.HandleTrigger();
            target.transform.position = gaze.hitPosition;
        }

    }
}