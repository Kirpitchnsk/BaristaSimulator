using UnityEngine;


namespace Arenar.AudioSystem
{
    public class AudioController
    {
        private AudioSource audioSource = default;


        public AudioController(AudioSource audioSource) =>
            this.audioSource = audioSource;


        public void PlaySound(AudioClip clip, bool loop = false)
        {
            if (audioSource == null || clip == null)
                return;
            
            audioSource.clip = clip;
            audioSource.loop = loop;
            audioSource.Play();
        }

        public void StopSound()
        {  
            if (audioSource == null)
                return;
            
            audioSource.Stop();
        }
    }
}