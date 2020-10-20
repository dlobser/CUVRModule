using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CUVR
{
    public class Interactable_UnityEvents : Interactable
    {
        public UnityEvent hoverEvent;
        public UnityEvent triggerEvent;
        public UnityEvent enterEvent;
        public UnityEvent exitEvent;

        bool trigger;


        public override void HandleHover()
        {
            base.HandleHover();
            hoverEvent.Invoke();
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

        public override void HandleExit()
        {
            base.HandleHover();
            exitEvent.Invoke();
        }

        public override void HandleEnter()
        {
            base.HandleHover();
            enterEvent.Invoke();
        }

        public override void HandleTrigger()
        {
            base.HandleHover();
            triggerEvent.Invoke();
        }
    }
}