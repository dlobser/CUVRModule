using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR{

    public class Interactable_ChangeColor : Interactable
    {
        public Color color;
        Color oldColor;
        [Tooltip("Leave empty to make this object the target")]
        public GameObject target;

    	public override void HandleStart()
    	{
    		base.HandleStart();
            if (target == null)
                target = this.gameObject;
            SetOldColor();
    	}

    	public override void HandleHover()
    	{
            base.HandleHover();
            if(hoverCounter<hoverTime)
                SetColor(Color.Lerp(oldColor, color, hoverCounter/hoverTime));

    	}

    	public override void HandleWaiting()
    	{
            base.HandleWaiting();

            if (hoverCounter > 0)
            {
                SetColor(Color.Lerp(oldColor, color, hoverCounter));
            }
            if (debug)
                Debug.Log(Color.Lerp(oldColor, color, hoverCounter));

    	}

    	private void OnApplicationQuit()
    	{
            SetColor(oldColor);
    	}

        void SetOldColor(){
            if (target.GetComponent<MeshRenderer>() != null)
                oldColor = target.GetComponent<MeshRenderer>().material.color;
            else if (target.GetComponent<SpriteRenderer>() != null)
                oldColor = target.GetComponent<SpriteRenderer>().color;

        }

        void SetColor(Color color){
            if (target.GetComponent<MeshRenderer>() != null)
                target.GetComponent<MeshRenderer>().material.color = color;
            else if (target.GetComponent<SpriteRenderer>() != null)
                target.GetComponent<SpriteRenderer>().color = color;
        }
    }


}