using Unity.VisualScripting;
using UnityEngine;
using Zenject;


namespace Arenar.AudioSystem
{
    public class UiSoundManager : IUiSoundManager
    {
        private Camera camera;
        private AudioController uiSoundController;
        private UiSoundsLibrary uiSoundLibrary;


        [Inject]
        public void Construct(IAudioSystemManager audioSystemManager,
                              IAmbientManager ambientManager,
                              AudioLibrary soundsLibrary,
                              Camera camera)
        {
            this.camera = camera;
            AudioSource uiSoundSource =
                audioSystemManager.CreateAudioSource(camera.gameObject, AudioSystemType.UI);

            Initialize(uiSoundSource, soundsLibrary);
        }
        
        public void Initialize(AudioSource uiSoundSource, AudioLibrary audioLibrary)
        {
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
