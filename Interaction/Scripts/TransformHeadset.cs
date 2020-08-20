using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR{

    public class TransformHeadset : MonoBehaviour
    {
        public Button button;
        public float rotationSpeed;
        public float translateSpeed;
        float tSpeed;
        Vector3 move;
        public float easeSpeed = .5f;

        void Update()
        {
            if (button.click > .5f)
            {
                tSpeed = Mathf.Lerp(tSpeed, translateSpeed, easeSpeed);
                this.transform.Translate(Camera.main.transform.forward * tSpeed * Time.deltaTime);
            }
            else if(tSpeed>0){
                tSpeed = Mathf.Lerp(tSpeed, 0, easeSpeed);
                this.transform.Translate(Camera.main.transform.forward*tSpeed*Time.deltaTime);
            }
        }
    }


}