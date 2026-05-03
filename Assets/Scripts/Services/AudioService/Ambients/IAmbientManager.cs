using UnityEngine;

namespace Arenar.AudioSystem
{
    public interface IAmbientManager
    {
        public AudioSource AmbientAudioSource { get; }


        void PlayAmbient(AmbientType ambientType, bool loop = true);

        void StopAmbient();
    }
}
