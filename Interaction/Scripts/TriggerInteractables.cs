using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR{

    public class TriggerInteractables : Interactable
    {
        public bool trigger;
        public Interactable[] interactables;

        //void Update()
        //{
        //    base.up
        //    //if (trigger)
        //    //{
        //    //    foreach(Interactable i in interactables)
        //    //    {
        //    //        i.HandleTrigger();
        //    //    }
        //    //}
        //}

        public override void HandleHover()
        {
            base.HandleHover();
            if (clicked > .5f && !trigger)
            {
                HandleTrigger();
                trigger = true;
            }
            else if (clicked < .5f && trigger)
            {
                trigger = false;
            }
        }

        public override void HandleTrigger()
        {
            base.HandleTrigger();
            foreach (Interactable i in interactables)
            {
                i.HandleTrigger();
            }
        }
    }
}