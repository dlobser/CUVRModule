using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR
{
    public class Interactable_HoverStates : Interactable
    {
        public Sprite waiting;
        public Sprite click;
        public Sprite hover;
        public SpriteRenderer spriteRenderer;

        void Start()
        {
            spriteRenderer.sprite = waiting;
        }

        public override void HandleHover()
        {
            base.HandleHover();
            if (clicked > .5f)
            {
                spriteRenderer.sprite = click;
            }
            else
            {
                spriteRenderer.sprite = hover;
            }
        }

        public override void HandleExit()
        {
            base.HandleExit();
            spriteRenderer.sprite = waiting;
        }
    }
}