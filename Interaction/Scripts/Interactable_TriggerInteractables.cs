using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR{
    public class Interactable_TriggerInteractables : Interactable
    {
        public Interactable[] trigger;

        public override void HandleEnter()
        {
            base.HandleExit();
            HandleTrigger();
        }

        public override void HandleTrigger()
        {
            base.HandleTrigger();
            foreach (Interactable g in trigger)
                g.HandleTrigger();

        }
    }
}