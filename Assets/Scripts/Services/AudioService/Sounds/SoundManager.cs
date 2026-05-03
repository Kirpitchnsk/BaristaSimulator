using UnityEngine;


namespace Arenar.AudioSystem
{
    public class SoundManager : ISoundManager
    {
        public void PlaySound(AudioSource audioSource, AudioClip clip, bool loop = false)
        {
            audioSource.clip = clip;
            audioSource.loop = loop;
            audioSource.Play();
        }

        public void StopSound(AudioSource audioSource)
        {
            audioSource.Stop();
        }
    }
}