using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR
{
    public class ScaleWithTrigger : MonoBehaviour
    {
        public Button button;
        public Vector2 scale;
        public GameObject target;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            float s = Mathf.Lerp(scale.x, scale.y, button.click);
            Vector3 sc = new Vector3(s, s, s);
            target.transform.localScale = sc;
        }
    }
}