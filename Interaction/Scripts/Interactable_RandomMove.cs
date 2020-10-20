using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CUVR;

namespace lobser
{
    public class Interactable_RandomMove : Interactable
    {
        public override void HandleEnter()
        {
            base.HandleEnter();
            HandleTrigger();
        }

        public override void HandleTrigger()
        {
            base.HandleTrigger();
            Vector3 randomPos = Random.insideUnitSphere.normalized;
            this.transform.localPosition += randomPos;
        }
    }
}