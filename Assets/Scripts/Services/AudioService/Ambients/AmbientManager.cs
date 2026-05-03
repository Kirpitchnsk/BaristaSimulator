using SibGameJam2026.Cameras;
using UnityEngine;


namespace Arenar.AudioSystem
{
    public class AmbientManager : IAmbientManager
    {
        private AmbientType lastAmbientType = AmbientType.None;
        private AudioController audioController;
        private AmbientLibrary ambientLibrary;
        private readonly ICameraService _cameraService;
        private readonly IAudioSystemManager _audioSystemManager;


        public AudioSource AmbientAudioSource { get; private set; }


        public AmbientManager(AudioLibrary audioLibrary,
							  ICameraService cameraService,
							  IAudioSystemManager audioSystemManager) {
            _cameraService = cameraService;
            _audioSystemManager = audioSystemManager;
            ambientLibrary = audioLibrary.AmbientLibrary;
            lastAmbientType = AmbientType.None;
        }


        public void PlayAmbient(AmbientType ambientType, bool loop = true) {
            if (AmbientAudioSource == null)
            {
                _cameraService.TryGetActiveCamera(out var cameraController);
                AmbientAudioSource = _audioSystemManager.CreateAudioSource(cameraController.gameObject, AudioSystemType.Music);
                audioController = new AudioController(AmbientAudioSource);
            }

            if (lastAmbientType == ambientType)
                return;

            lastAmbientType = ambientType;
            audioController.PlaySound(ambientLibrary.GetAmbientByType(ambientType), loop);
        }

        public void StopAmbient()
        {
            lastAmbientType = AmbientType.None;
            audioController.StopSound();
        }
    }
}
