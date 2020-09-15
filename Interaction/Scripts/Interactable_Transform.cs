using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR{
    public class Interactable_Transform : Interactable
    {
        public GameObject target;

        public Vector3 position;
        public Vector3 scale;
        public Vector3 rotation;

        Vector3 initPosition;
        Vector3 initScale;
        Vector3 initRotation;

        public Interactable[] triggerOnFinish;
        public Interactable[] triggerOnStart;

        bool swapper;
        public bool swap;

        public float speed;
        public bool trigger = false;

        private void LateUpdate()
        {
            if (trigger)
            {
                HandleTrigger();
                trigger = false;
            }
        }

        public override void HandleStart()
        {
            base.HandleStart();
            if (target == null)
                target = this.gameObject;
            initPosition = target.transform.localPosition;
            initScale = target.transform.localScale;
            initRotation = target.transform.localEulerAngles;
        }

        public override void HandleTrigger()
        {
            base.HandleTrigger();
            StartCoroutine(Move());
        }

        IEnumerator Move()
        {
            float wait = 0;

            if (triggerOnStart.Length > 0)
            {
                foreach (Interactable i in triggerOnStart)
                {
                    if (!i.gameObject.activeInHierarchy)
                        i.gameObject.SetActive(true);
                    if(i.gameObject.GetComponent<Interactable_Transform>()!=null)
                        wait = i.gameObject.GetComponent<Interactable_Transform>().speed;
                    i.HandleTrigger();
                }
            }
            

            yield return new WaitForSeconds(wait);
            float counter = 0;
            while(counter < 1)
            {
                counter += Time.deltaTime / speed;
                float c = Mathf.SmoothStep(0, 1, counter);
                float iCounter = (1 - c);

                target.transform.localPosition = Vector3.Lerp(initPosition, position, !swapper ? c : iCounter);
                target.transform.localScale = Vector3.Lerp(initScale, scale, !swapper ? c : iCounter);
                target.transform.localEulerAngles = Vector3.Lerp(initRotation, rotation, !swapper ? c : iCounter);
                yield return null;
            }
             

            if (triggerOnFinish.Length > 0)
            {
                foreach (Interactable i in triggerOnFinish)
                {
                    if (!i.gameObject.activeInHierarchy)
                        i.gameObject.SetActive(true);
                    i.HandleTrigger();
                }
            }


            if (swap)
            {
                swapper = !swapper;
                Interactable[] temp = triggerOnStart;
                triggerOnStart = triggerOnFinish;
                triggerOnFinish = temp;
            }
        }
    }
}