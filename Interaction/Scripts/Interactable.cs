using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR{
    public class Interactable : MonoBehaviour
    {
        
        bool ping = false;
        bool prevPing = false;

        enum State { Enter, Hover, Exit, Waiting, Trigger };
        State state;

        public RaycastInteraction gaze { get; set; }
        public float minDistance = 1e6f;

        public bool debug;

        public float clicked { get; set; }
        public string type;
        public float hoverCounter { get; set; }
        public float hoverTime = 1;

    	private void Start()
    	{
            HandleStart();
    	}

    	void Update()
        {
            HandleUpdate();
        }

        public virtual void HandleStart(){
            state = new State();
        }

        public virtual void HandleUpdate(){
            if (!ping && !prevPing)
            {
                state = State.Waiting;
            }
            else if (ping && !prevPing)
            {
                state = State.Enter;
            }
            else if (ping && prevPing)
            {
                state = State.Hover;
            }
            else if (!ping && prevPing)
            {
                state = State.Exit;
            }

            if (state == State.Enter)
                HandleEnter();
            if (state == State.Hover)
                HandleHover();
            if (state == State.Exit)
                HandleExit();
            if (state == State.Waiting)
                HandleWaiting();
            if (state == State.Trigger)
                HandleTrigger();

            prevPing = ping;
            ping = false;
        }

        public virtual void Ping(RaycastInteraction raycaster, float click, string whatType){
            gaze = raycaster;
            if(whatType.Equals(type)){
                ping = true;
                clicked = click;
            }
        }

        public virtual void HandleEnter(){
            if (debug)
                Debug.Log("enter");
        }

        public virtual void HandleHover(){
            if (debug)
                Debug.Log("hover");
            if (hoverCounter < hoverTime)
                hoverCounter += Time.deltaTime;
        }

        public virtual void HandleExit(){
            if (debug)
                Debug.Log("exit");
        }

        public virtual void HandleWaiting(){
            if (hoverCounter > 0)
                hoverCounter -= Time.deltaTime;
            if (debug)
                Debug.Log(this.gameObject.name + " is waiting");
        }

        public virtual void HandleTrigger()
        {
            if (debug)
                Debug.Log("trigger");
        }
    }
}