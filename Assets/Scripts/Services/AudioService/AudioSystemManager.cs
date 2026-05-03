using Arenar.Options;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;


namespace Arenar.AudioSystem
{
    public class AudioSystemManager : IAudioSystemManager
    {
        private const float DB_MINIMUM = -80f;
        private const float DB_MAXIMUM = 0f;


        private AudioMixerData audioMixerData;
        private IOptionsController optionsController;
        private IUiSoundManager uiSoundManager;
        private Tween disableSoundsTween;


        public AudioSystemManager(AudioMixerData audioMixerData,
                                  IOptionsController optionsController)
        {
            this.audioMixerData = audioMixerData;
            this.optionsController = optionsController;
            DOVirtual.DelayedCall(0.0f, Initialize);
        }


        public void Initialize()
        {
            EnableAudio();
        }

        public AudioSource CreateAudioSource(GameObject audioSourceParent, AudioSystemType type)
        {
            AudioSource audioSource = audioSourceParent.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.outputAudioMixerGroup = GetAudioMixerGroup(type);

            return audioSource;
        }

        public void DisableAudio(bool withSave = false)
        {
            if (withSave)
            {
                
            }
            else
            {
                SetVolume(AudioSystemType.Music, false, 0);
                SetVolume(AudioSystemType.Sound, false, 0);
                SetVolume(AudioSystemType.UI, false, 0);
            }
        }

        public void EnableAudio()
        {
            MusicOption musicOption = optionsController.GetOption<MusicOption>();
            SetVolume(AudioSystemType.Music, musicOption.IsActive, musicOption.Volume);

            SoundOption soundOption = optionsController.GetOption<SoundOption>();
            SetVolume(AudioSystemType.Sound, soundOption.IsActive, soundOption.Volume);

            UiSoundOption uiSoundOption = optionsController.GetOption<UiSoundOption>();
            SetVolume(AudioSystemType.UI, uiSoundOption.IsActive, uiSoundOption.Volume);
        }

        public void SetVolume(AudioSystemType type, bool status, float volume)
        {
            if (audioMixerData == null)
            {
                Debug.LogError("Not found audio mixer group. Abort!");
                return;
            }

            audioMixerData.AudioGroupNames.TryGetValue(type, out string audioGroupName);

            if (string.IsNullOrEmpty(audioGroupName))
            {
                Debug.LogError($"Not found audio mixer group name for sound type {type}. Abort!");
                return;
            }

            if (!status)
                volume = 0f;

            if (!audioMixerData.AudioMixerGroup.audioMixer.SetFloat(audioGroupName,
                    Mathf.Lerp(DB_MINIMUM, DB_MAXIMUM, volume)))
            {
                Debug.LogError($"Not found {audioGroupName} audio type!");
            }
        }

        public AudioMixerGroup GetAudioMixerGroup(AudioSystemType type)
        {
            audioMixerData.AudioGroupNames.TryGetValue(type, out string audioGroupName);

            if (string.IsNullOrEmpty(audioGroupName))
            {
                Debug.LogError($"Not found audio mixer group name for sound type {type}. Abort!");
                return null;
            }

            return audioMixerData.AudioMixerGroup.audioMixer.FindMatchingGroups(audioGroupName)[0];
        }
    }
}
