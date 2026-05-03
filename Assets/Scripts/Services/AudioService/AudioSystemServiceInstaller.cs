using UnityEngine;
using Zenject;


namespace Arenar.AudioSystem
{
    public class AudioSystemServiceInstaller : MonoInstaller
    {
        [SerializeField] private AudioMixerData audioMixerData = default;
        [SerializeField] private AudioLibrary audioLibrary;
        
        
        public override void InstallBindings()
        {
            InstallLibrary();
            InstallMixerData();
            InstallManagers();
            InstallAudioSystem();
        }
        
        private void InstallMixerData()
        {
            Container.BindInstance(audioMixerData);
        }

        private void InstallLibrary()
        {
            Container.BindInstance(audioLibrary)
                .AsSingle()
                .NonLazy();
        }

        private void InstallManagers()
        {
            Container.Bind<IAmbientManager>()
                .To<AmbientManager>()
                .AsSingle()
                .NonLazy();
            
            Container.Bind<IUiSoundManager>()
                .To<UiSoundManager>()
                .AsSingle()
                .NonLazy();
            
            Container.Bind<ISoundManager>()
                .To<SoundManager>()
                .AsSingle()
                .NonLazy();
        }

        private void InstallAudioSystem()
        {
            Container.Bind<IAudioSystemManager>()
                .To<AudioSystemManager>()
                .AsSingle()
                .NonLazy();
        }
    }
}
