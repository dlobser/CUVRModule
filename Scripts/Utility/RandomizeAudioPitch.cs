using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeAudioPitch : MonoBehaviour
{
    public float min;
    public float max;

    void Start()
    {
        this.GetComponent<AudioSource>().pitch = Random.Range(min, max);
    }

}
