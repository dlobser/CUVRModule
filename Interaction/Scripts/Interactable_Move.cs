using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR{

    public class Interactable_Move : Interactable
    {

        public Vector3 moveTo;

        public override void HandleHover()
        {
            this.transform.position = moveTo;
        }
    	
    }


}