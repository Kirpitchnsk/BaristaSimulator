using UnityEngine;


namespace Arenar.AudioSystem
{
    public interface IUiSoundManager
    {
        void Initialize(AudioSource uiSoundSource, AudioLibrary audioLibrary);

        void PlaySound(UiSoundType type);

        void StopAllSounds();
    }
}
