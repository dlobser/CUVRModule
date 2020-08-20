using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR{

    public class Interactable_Destroy : Interactable
    {
        //explodes an object when clicked

        public GameObject explosion;
        public GameObject target;
        GameObject e;

        public override void HandleHover()
        {
            if(clicked>.5f){
                HandleTrigger();
            }
        }

    	public override void HandleTrigger()
    	{
    		base.HandleTrigger();
            if (explosion != null)
            {
                e = Instantiate(explosion);
                e.transform.position = this.transform.position;
            }
            if (target != null)
                Destroy(target);
            else
                Destroy(this.gameObject);
    	}
    }
}