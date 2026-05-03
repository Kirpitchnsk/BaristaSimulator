using UnityEngine;


namespace Arenar.AudioSystem
{
    public class AmbientManager : IAmbientManager
    {
        private AmbientType lastAmbientType = AmbientType.None;
        private AudioController audioController;
        private AmbientLibrary ambientLibrary;


        public AudioSource AmbientAudioSource { get; }


        public AmbientManager(AudioLibrary audioLibrary,
							  Camera camera,
							  IAudioSystemManager audioSystemManager)
        {
            AmbientAudioSource = audioSystemManager.CreateAudioSource(camera.gameObject, AudioSystemType.Music);
            audioController = new AudioController(AmbientAudioSource);
            ambientLibrary = audioLibrary.AmbientLibrary;
            lastAmbientType = AmbientType.None;
        }


        public void PlayAmbient(AmbientType ambientType, bool loop = true)
        {
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
