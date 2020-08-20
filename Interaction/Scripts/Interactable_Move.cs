using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR{

    public class Interactable_Move : Interactable
    {
        //navigate to the raycast position
        public GameObject target;
        bool trigger;
        bool prevTrigger;

        public override void HandleHover()
        {
            if (clicked > .5f)
            {
                trigger = true;
            }
            else
            {
                trigger = false;
            }

            if (trigger && !prevTrigger)
            {
                HandleTrigger();
            }

            print(trigger);
        }

        public override void HandleTrigger()
        {
            base.HandleTrigger();
            target.transform.position = gaze.hitPosition;
        }

        public override void HandleUpdate()
        {
            base.HandleUpdate();
            prevTrigger = trigger;

            trigger = false;
        }
    }
}