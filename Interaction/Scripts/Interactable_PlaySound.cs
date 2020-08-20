using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR{

    public class Interactable_PlaySound : Interactable
    {
        public AudioSource sound;
        public AudioClip clip;
        [Tooltip("Low and High values")]
        public Vector2 randomizePitch;

    	public override void HandleEnter()
    	{
            base.HandleEnter();
            if (clip != null) { 
                sound.clip = clip;
                sound.pitch = Random.Range(randomizePitch.x, randomizePitch.y);
                sound.Play();
            }
    	}

        public override void HandleTrigger()
        {
            base.HandleTrigger();
            HandleEnter(); 
        }
    }
}