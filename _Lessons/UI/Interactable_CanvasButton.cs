using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CUVR
{
    public class Interactable_CanvasButton : Interactable
    {
        UnityEngine.UI.Button button;
        PointerEventData p;
        public GameObject target;
        bool triggered = false;

        private void Start()
        {
            button = GetComponent<UnityEngine.UI.Button>();
            //you don't need an active event system - null is ok
            p = new PointerEventData(EventSystem.current);
        }

        public override void HandleEnter()
        {
            base.HandleEnter();
            button.OnPointerEnter(p);
        }

        public override void HandleExit()
        {
            base.HandleExit();
            button.OnPointerExit(p);
            button.OnPointerUp(p);
            triggered = false;
        }

        public void Hover()
        {

        }

        public void Down()
        {
            button.OnPointerDown(p);
        }

        public void Up()
        {
            button.OnPointerUp(p);

        }

        public override void HandleTrigger()
        {
            base.HandleTrigger();
            Down();
            Clicker();
        }

        public override void HandleHover()
        {
            base.HandleHover();
            if (clicked > .5f && !triggered)
            {
                HandleTrigger();
                triggered = true;
            }
            else if(clicked<.5f && triggered)
            {
                Up();
                triggered = false;
            }

        }

        public void Clicker()
        {
            p.button = PointerEventData.InputButton.Left;
            p.pointerPress = target;
            p.eligibleForClick = true;
            button.OnPointerClick(p);

        }
    }
}