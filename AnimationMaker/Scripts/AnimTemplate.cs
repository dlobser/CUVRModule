using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Art
{
    public class AnimTemplate : Anim
    {

        public GameObject root { get; set; }

        void Update()
        {
            if (Input.GetKey(KeyCode.R))
                rebuild = true;
            if (Input.GetKey(KeyCode.D))
                Destroy(root);
            
            if (rebuild)
            {
                Rebuild();
                rebuild = false;
            }

            counter += Time.deltaTime * speed;

            if(root!=null)
                Animate();
        }

        public override void Rebuild()
        {
            if (root != null)
            {
                Destroy(root);
            }

            root = new GameObject();
            root.name = "ArtMaker_Root";
            MakeArt();
        }

        public virtual void MakeArt()
        {
            /*
             * Make your art in this function
             * Make every new object the child of root.transform
             * "yourGameObject.transform.SetParent(root.transform);"
             * or
             * "yourGameObject.transform.parent = root.transform;
             */
        }

	}
}