using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManagerView : EventView
    {
        public AudioClip Click;
        public AudioClip Drop;
        private AudioSource source;

        public void Start()
        {
            source = GetComponent<AudioSource>();
        }

        private void PlayClip(AudioClip clip)
        {
            source.clip = clip;
            source.Play();
        }

        public void ClickPlay()
        {
            PlayClip(Click);
        }

        public void DropPlay()
        {
            PlayClip(Drop);
        }
    }
}