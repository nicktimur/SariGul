using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarSound : MonoBehaviour
{
    public static WarSound instance { get; private set; }
    private AudioSource source;
    private void Awake()
    {
        source = GetComponent<AudioSource>();

        //Keep this object even when we go to new scene
        if (instance == null)
        {
            instance = this;
        }
    }

        public void PlaySound(AudioClip _sound)
    {
        source.PlayOneShot(_sound);
    }
}
