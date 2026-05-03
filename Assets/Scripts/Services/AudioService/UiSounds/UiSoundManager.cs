using SibGameJam2026.Cameras;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;


namespace Arenar.AudioSystem
{
    public class UiSoundManager : IUiSoundManager
    {
        private AudioController uiSoundController;
        private UiSoundsLibrary uiSoundLibrary;
        private  AudioSource uiSoundSource;

        private IAudioSystemManager audioSystemManager;
        private ICameraService camaraService;


        [Inject]
        public void Construct(IAudioSystemManager audioSystemManager,
                              IAmbientManager ambientManager,
                              AudioLibrary soundsLibrary,
                              ICameraService camaraService)
        {

            this.audioSystemManager = audioSystemManager;
            this.camaraService = camaraService;
            Initialize(uiSoundSource, soundsLibrary);
        }
        
        public void Initialize(AudioSource uiSoundSource, AudioLibrary audioLibrary)
        {
            uiSoundSource =
                audioSystemManager.CreateAudioSource(new GameObject("UI SOUND POINT"), AudioSystemType.UI);
            
            uiSoundController = new AudioController(uiSoundSource);
            uiSoundLibrary = audioLibrary.UiSoundsLibrary;
        }

        public void PlaySound(UiSoundType type) =>
            PlaySound(uiSoundLibrary.UiSounds[type]);

        public void StopAllSounds() =>
            uiSoundController.StopSound();

        private void PlaySound(AudioClip sound)
        {
            if (uiSoundController != null
                && uiSoundLibrary != null)
                uiSoundController.PlaySound(sound);
        }
    }
}
