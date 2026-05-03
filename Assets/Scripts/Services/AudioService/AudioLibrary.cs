using UnityEngine;
using Zenject;


namespace Arenar.AudioSystem
{
    [CreateAssetMenu(menuName = "Sound system/Sound Library")]
    public class AudioLibrary : ScriptableObjectInstaller<AudioLibrary>
    {
        [SerializeField] private AmbientLibrary ambientLibrary;
        [SerializeField] private UiSoundsLibrary uiSoundsLibrary;
        [SerializeField] private SoundsLibrary soundsLibrary;


        public AmbientLibrary AmbientLibrary => ambientLibrary;

        public UiSoundsLibrary UiSoundsLibrary => uiSoundsLibrary;

        public SoundsLibrary SoundsLibrary => soundsLibrary;
    }
}
