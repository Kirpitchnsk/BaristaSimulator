using UnityEngine;


namespace Arenar.AudioSystem
{
    public interface ISoundManager
    {
        void PlaySound(AudioSource audioSource, AudioClip clip, bool loop = false);

        void StopSound(AudioSource audioSource);
    }
}