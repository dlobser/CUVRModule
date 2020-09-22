using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR
{
    public class Interactable_LerperWithCoroutines : Interactable
    {
        public Transform lerpTo;
        public Transform lerpFrom;
        public Transform target;
        bool triggered = false;

        public Color A;
        public Color B;

        Coroutine C;

        public AudioSource audioSource;

        public float speed;

        public override void HandleTrigger()
        {
            base.HandleTrigger();
            C = StartCoroutine(MoveTo());
        }

        public override void HandleHover()
        {
            base.HandleHover();
            if (clicked > .5f)
            {
                if (C != null)
                    StopCoroutine(C);

                HandleTrigger();
            }
        }

        IEnumerator MoveTo()
        {
            audioSource.Play();
            audioSource.pitch = Random.value;
            triggered = true;
            float counter = 0;
            Vector3 startPosition = target.position;
            while(counter < 1)
            {
                counter += Time.deltaTime/speed;
                target.transform.position =   Vector3.Lerp(lerpFrom.position, lerpTo.position, counter);
                target.transform.localScale = Vector3.Lerp(lerpFrom.localScale, lerpTo.localScale, counter);
                target.transform.rotation =   Quaternion.Lerp(lerpFrom.rotation, lerpTo.rotation, counter);
                target.GetComponent<MeshRenderer>().material.color = Color.Lerp(A, B, counter);
                yield return null;
            }
            triggered = false;

        }
    }

}