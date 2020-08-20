using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR{

    public class TriggerInteractables : Interactable
    {
        public bool trigger;
        public Interactable[] interactables;

        void Update()
        {
            if (trigger)
            {
                foreach(Interactable i in interactables)
                {
                    i.HandleTrigger();
                }
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
            trigger = false;
        }
    }
}