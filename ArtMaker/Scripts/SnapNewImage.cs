using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CUVR
{
    public class SnapNewImage : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData pointerEventData)
        {
            Snap();
        }

        public void Snap()
        {
            if (FindObjectOfType<ArtMaker>() != null)
            {
                RenderTexture renderTexture = new RenderTexture((int)(2048*Camera.main.aspect), 2048, 0);
                Camera.main.targetTexture = renderTexture;
                Camera.main.depth = 2;
                Camera.main.Render();
                FindObjectOfType<ArtMaker>().Rebuild();
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", renderTexture);
                Camera.main.targetTexture = null;
                Camera.main.depth = -1;
            }
            else{
                Debug.LogWarning("ArrtMaker not found");
            }
        }
    }
}