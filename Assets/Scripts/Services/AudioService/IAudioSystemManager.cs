using UnityEngine;
using UnityEngine.Audio;

namespace Arenar.AudioSystem
{
    public interface IAudioSystemManager
    {
        void Initialize();

        void SetVolume(AudioSystemType type, bool status, float volume);

        void DisableAudio(bool withSave = false);

        void EnableAudio();

		AudioMixerGroup GetAudioMixerGroup(AudioSystemType type);

        public AudioSource CreateAudioSource(GameObject audioSourceParent, AudioSystemType type);
    }
}
