using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR
{
    public class Interactable_ReRender : Interactable
    {

        private void Awake()
        {
            //this.debug = true;
        }
        public override void HandleHover()
        {
            if (clicked > .5f)
            {
                Snap();
                print("snapshot");
            }

        }

        void Snap()
        {
            RenderTexture renderTexture = new RenderTexture(2048, 2048, 0);
            Camera.main.targetTexture = renderTexture;
            Camera.main.depth = 2;
            Camera.main.Render();
            FindObjectOfType<ArtSquare>().rebuild = true;
            //FindObjectOfType<Art.ArtSquare>().Update();
            GetComponent<MeshRenderer>().material.SetTexture("_MainTex", renderTexture);
            Camera.main.targetTexture = null;
            Camera.main.depth = -1;

        }

    }
}