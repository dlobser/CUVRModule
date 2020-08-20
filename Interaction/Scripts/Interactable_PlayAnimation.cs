using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR{

    public class Interactable_PlayAnimation : Interactable
    {

        public Animator animator;
        public string trigger;

        public override void HandleHover()
        {
            if(clicked>.5f){
                HandleTrigger();
            }
        }

    	public override void HandleTrigger()
    	{
    		base.HandleTrigger();
            animator.SetTrigger(trigger);
    	}
    }


}